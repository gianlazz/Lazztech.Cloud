using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Events.Domain
{
    public class MentorRequestConductor
    {
        public Dictionary<string, MentorRequest> Requests { get; private set; }

        private readonly IRepository _db;
        private readonly ISmsService _sms;
        private readonly IRequestNotifier _Notifier;

        public MentorRequestConductor(IRepository repository, ISmsService sms, IRequestNotifier requestResponder)
        {
            _db = repository;
            _sms = sms;
            _Notifier = requestResponder;
            Requests = new Dictionary<string, MentorRequest>();
        }

        public bool TryAddRequest(MentorRequest request)
        {
            var requestedMentorId = request.Mentor.PhoneNumber;
            if (Requests.ContainsKey(requestedMentorId))
                return false;
            else
            {
                Requests.Add(requestedMentorId, request);
                AddMenorRequestDb(request);
                StartRequestTimeOutAsync(request);
                return true;
            }
        }

        public MentorRequest ProcessResponse(SmsDto inboundSms)
        {
            AddSmsDb(inboundSms);
            
            var matchingRequest = FindResponseRequest(inboundSms);
            if (IsAcceptanceResponse(inboundSms))
            {
                HandleRequestAcceptance(inboundSms, matchingRequest);
                AcceptanceResponseConfirmation(inboundSms, matchingRequest);
                StartMentorReservationTimeoutAsync(matchingRequest);
            }
            else if (IsRejectionResponse(inboundSms))
            {
                ResponseProcessedConfirmation(inboundSms);
                HandleRequestRejection(inboundSms, matchingRequest);
            }
            else
            {
                HandleUnidentifiedRequestResponse(inboundSms, matchingRequest);
            }
            
            if (inboundSms.DateTimeWhenProcessed == null)
                HandleResponseWithNoRequest(inboundSms);

            return matchingRequest;
        }

        private MentorRequest FindResponseRequest(SmsDto inboundSms)
        {
            var MatchingRequests = Requests.Values.Where(x => x.DateTimeWhenProcessed == null
                &&
                x.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber).ToList();

            if (MatchingRequests.Count() > 1) { throw new Exception(); }

            return MatchingRequests.FirstOrDefault();
        }

        private async Task StartRequestTimeOutAsync(MentorRequest request)
        {
            await Task.Delay(request.RequestTimeout);
            request.TimedOut = true;
            request.DateTimeWhenProcessed = DateTime.Now;
            UpdateMentoRequestDb(request);
            Requests.Remove(request.Mentor.PhoneNumber);
            NotifyMentorOfRequestTimeout(request.Mentor);
        }

        private async Task StartMentorReservationTimeoutAsync(MentorRequest request)
        {
            if (request.MentoringDuration != null)
            {
                await Task.Delay(request.MentoringDuration);
                var mentor = request.Mentor;
                mentor.IsAvailable = true;
                UpdateMentorDb(mentor);
                NotifyResponseTimeUp(request);
            }
        }

        private void HandleResponseWithNoRequest(SmsDto inboundSms)
        {
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            UnIdentifiedResponse(inboundSms);
            UpdateSmsDb(inboundSms);
        }

        private void HandleUnidentifiedRequestResponse(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            UnIdentifiedResponse(inboundSms, mentorRequest.OutboundSms);
            UpdateSmsDb(inboundSms);
        }

        private void HandleRequestRejection(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            mentorRequest.RequestAccepted = false;
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.InboundSms = inboundSms;

            UpdateMentoRequestDb(mentorRequest);
            UpdateSmsDb(inboundSms);

            //_Notifier.UpdateMentorRequestee(mentorRequest);
        }

        private void HandleRequestAcceptance(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            mentorRequest.RequestAccepted = true;
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.InboundSms = inboundSms;
            mentorRequest.Mentor.IsAvailable = false;

            UpdateSmsDb(inboundSms);
            UpdateMentorDb(mentorRequest.Mentor);
            UpdateMentoRequestDb(mentorRequest);

            //_Notifier.UpdateMentorRequestee(mentorRequest);
        }

        #region MessageInterpretationHelperMethods

        private bool IsAcceptanceResponse(SmsDto sms)
        {
            if (sms.MessageBody.Trim().ToLower() == "y" ||
                sms.MessageBody.Trim().ToLower() == "yes")
                return true;

            return false;
        }

        private bool IsRejectionResponse(SmsDto sms)
        {
            if (sms.MessageBody.Trim().ToLower() == "n" ||
                sms.MessageBody.Trim().ToLower() == "no")
                return true;

            return false;
        }

        #endregion MessageInterpretationHelperMethods

        #region MessageResponseHelperMethods

        private void AcceptanceResponseConfirmation(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            string message = $"Response confirmed. " +
                $"You've been marked as busy for {mentorRequest.MentoringDuration.Minutes} minutes" +
                $" and will automatically be set as available again after.";
            _sms.SendSms(inboundSms.FromPhoneNumber, message);
        }

        private void ResponseProcessedConfirmation(SmsDto sms)
        {
            string message = $"Response confirmed.";
            _sms.SendSms(sms.FromPhoneNumber, message);
        }

        private void UnIdentifiedResponse(SmsDto smsResponse, SmsDto lastSmsSent = null)
        {
            string message = $"Uncertain how to execute your objective.";
            _sms.SendSms(smsResponse.FromPhoneNumber, message);
            //RESEND THE INITAL PROMPT SMS
            if (lastSmsSent != null)
                _sms.SendSms(smsResponse.FromPhoneNumber, lastSmsSent.MessageBody);
        }

        private void NotifyResponseTimeUp(MentorRequest request)
        {
            _sms.SendSms(request.Mentor.PhoneNumber,
                                "Your reserved time is up and you've been marked as available. " +
                                "You may continue helping person(s) though until you've recieved another request.");
        }

        private void NotifyMentorOfRequestTimeout(Mentor mentor)
        {
            _sms.SendSms(mentor.PhoneNumber,
                "The request timeout duration has passed and you have been made available for other requests. " +
                "Please notify the responsable party if you would like to be marked as unavailable or busy");
        }

        #endregion MessageResponseHelperMethods

        #region DbHelperMethods

        private void AddSmsDb(SmsDto inboundSms)
        {
            _db.Add<SmsDto>(inboundSms);
        }

        private void AddMenorRequestDb(MentorRequest request)
        {
            _db.Add<MentorRequest>(request);
        }

        private void UpdateMentoRequestDb(MentorRequest request)
        {
            _db.Delete<MentorRequest>(x => x.Id == request.Id);
            _db.Add<MentorRequest>(request);
        }

        private void UpdateMentorDb(Mentor mentor)
        {
            _db.Delete<Mentor>(x => x.Id == mentor.Id);
            _db.Add<Mentor>(mentor);
        }

        private void UpdateSmsDb(SmsDto inboundSms)
        {
            _db.Delete<SmsDto>(x => x.GuidId == inboundSms.GuidId);
            _db.Add<SmsDto>(inboundSms);
        }

        #endregion DbHelperMethods
    }
}