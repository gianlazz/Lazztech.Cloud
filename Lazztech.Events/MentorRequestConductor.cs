using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Events.Domain
{
    public class MentorRequestConductor
    {
        public List<SmsDto> InboundMessages { get; private set; }
        public Dictionary<string, MentorRequest> UnprocessedRequests { get; private set; }
        public List<MentorRequest> ProcessedRequests { get; set; }

        private readonly IRepository _db;
        private readonly ISmsService _sms;
        private readonly IRequestNotifier _Notifier;

        public MentorRequestConductor(IRepository repository, ISmsService sms, IRequestNotifier requestResponder)
        {
            _db = repository;
            _sms = sms;
            _Notifier = requestResponder;
            InboundMessages = new List<SmsDto>();
            UnprocessedRequests = new Dictionary<string, MentorRequest>();
            ProcessedRequests = new List<MentorRequest>();
        }

        public bool TryAddRequest(MentorRequest request)
        {
            var requestedMentorId = request.Mentor.PhoneNumber;
            if (UnprocessedRequests.ContainsKey(requestedMentorId))
                return false;
            else
            {
                UnprocessedRequests.Add(requestedMentorId, request);
                AddMenorRequestDb(request);
                StartRequestTimeOutAsync(request);
                return true;
            }
        }

        public void ProcessInboundSms(SmsDto inboundSms)
        {
            AddSmsDb(inboundSms);
            var requestsToEvaluate = UnprocessedRequests.Values
                .Where(x => x.DateTimeWhenProcessed == null
                &&
                x.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber).ToList();
            foreach (var mentorRequest in requestsToEvaluate)
            {
                if (IsAcceptanceResponse(inboundSms))
                {
                    HandleRequestAcceptance(inboundSms, mentorRequest);
                    AcceptanceResponseConfirmation(inboundSms, mentorRequest);
                    StartMentorReservationTimeoutAsync(mentorRequest);
                    MoveToProcessed(mentorRequest);
                }
                else if (IsRejectionResponse(inboundSms))
                {
                    ResponseProcessedConfirmation(inboundSms);
                    HandleRequestRejection(inboundSms, mentorRequest);
                    MoveToProcessed(mentorRequest);
                }
                else
                {
                    HandleUnidentifiedRequestResponse(inboundSms, mentorRequest);
                }
            }

            if (inboundSms.DateTimeWhenProcessed == null)
                HandleResponseWithNoRequest(inboundSms);
        }

        private async Task StartRequestTimeOutAsync(MentorRequest request)
        {
            await Task.Delay(request.RequestTimeout);
            request.TimedOut = true;
            request.DateTimeWhenProcessed = DateTime.Now;
            MoveToProcessed(request);
            UpdateMentoRequestDb(request);
        }

        private async Task StartMentorReservationTimeoutAsync(MentorRequest request)
        {
            if (request.MentoringDuration != null)
            {
                await Task.Delay(request.MentoringDuration);
                var mentor = request.Mentor;
                mentor.IsAvailable = true;
                UpdateMentorDb(mentor);
                SendResponseTimeUpMessage(request);
            }
        }

        private void MoveToProcessed(MentorRequest request)
        {
            UnprocessedRequests.Remove(request.Mentor.PhoneNumber);
            ProcessedRequests.Add(request);
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

            _Notifier.UpdateMentorRequestee(mentorRequest);
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

            _Notifier.UpdateMentorRequestee(mentorRequest);
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

        #endregion

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

        private void SendResponseTimeUpMessage(MentorRequest request)
        {
            _sms.SendSms(request.Mentor.PhoneNumber,
                                "Your reserved time is up and you've been marked as available. " +
                                "You may continue helping person(s) though until you've recieved another request.");
        }

        #endregion

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
            _db.Delete<SmsDto>(x => x.Id == inboundSms.Id);
            _db.Add<SmsDto>(inboundSms);
        }

        #endregion
    }
}