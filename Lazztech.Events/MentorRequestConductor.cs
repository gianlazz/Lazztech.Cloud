using Lazztech.Events.Dal;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Events.Domain
{
    public class MentorRequestConductor : IMentorRequestConductor
    {
        private readonly IConductorDalHelper _db;
        private readonly ISmsService _sms;
        private readonly IRequestNotifier _Notifier;
        private readonly IMentorRequestsBackplane _requestsBackplane;

        public MentorRequestConductor(IConductorDalHelper dalHelper, ISmsService sms, IRequestNotifier requestResponder, IMentorRequestsBackplane requestsBackplane)
        {
            _db = dalHelper;
            _sms = sms;
            _Notifier = requestResponder;
            _requestsBackplane = requestsBackplane;
            //Requests = new Dictionary<string, MentorRequest>();
        }

        public void SubmitRequest(string uniqueRequesteeId, string teamName, string teamLocation, int mentorId)
        {
            if (_requestsBackplane.ContainsOutstandingRequestForMentor(mentorId) == false)
            {
                var mentor = _db.FindMentor(mentorId);
                if (mentor != null && mentor.IsAvailable && mentor.IsPresent)
                {
                    var message = EventStrings.OutBoundRequestSms(mentor.FirstName, teamName, teamLocation);
                    var request = new MentorRequest()
                    {
                        UniqueRequesteeId = uniqueRequesteeId,
                        Mentor = mentor,
                        OutboundSms = _sms.SendSms(mentor.PhoneNumber, message)
                    };
                    mentor.IsAvailable = false;
                    _db.UpdateMentorDb(mentor);
                    _requestsBackplane.AddMentorRequest(ref request);
                    StartRequestTimeOutAsync(request);
                }
            }
        }

        public MentorRequest ProcessResponse(SmsDto inboundSms)
        {
            _db.AddSmsDb(ref inboundSms);
            
            var matchingRequest = FindResponseRequest(inboundSms);
            var mentorFromDb = _db.FindMentorByPhoneNumber(inboundSms.FromPhoneNumber);
            if (mentorFromDb == null)
                return null;
            if (IsAcceptanceResponse(inboundSms) && matchingRequest != null)
            {
                HandleRequestAcceptance(inboundSms, matchingRequest);
                AcceptanceResponseConfirmation(inboundSms, matchingRequest);
                StartMentorReservationTimeoutAsync(matchingRequest);
            }
            else if (IsRejectionResponse(inboundSms) && matchingRequest != null)
            {
                HandleRequestRejection(inboundSms, matchingRequest);
                ResponseProcessedConfirmation(inboundSms);
            }
            else if (IsAvailableResponse(inboundSms))
            {
                HandleAvailableResponse(mentorFromDb, inboundSms);
                ResponseProcessedAvailable(inboundSms);
            }
            else if (IsBusyResponse(inboundSms))
            {
                HandleBusyResponse(mentorFromDb, inboundSms);
                ResponseProcessedBusy(inboundSms);
            }
            else if (IsGuideResponse(inboundSms))
            {
                HandleGuideResponse(inboundSms);
                ResponseProcessedGuide(inboundSms);
            }
            else if (mentorFromDb != null && matchingRequest != null)
            {
                HandleUnidentifiedRequestResponse(inboundSms, matchingRequest);
                ResponseUnIdentified(inboundSms, matchingRequest.OutboundSms);
            }

            if (inboundSms.DateTimeWhenProcessed == null)
                HandleResponseWithNoRequest(inboundSms);

            return matchingRequest;
        }

        #region HelperMethods

        private MentorRequest FindResponseRequest(SmsDto inboundSms)
        {
            return _requestsBackplane.FindResponseRequest(inboundSms);
        }

        private async Task StartRequestTimeOutAsync(MentorRequest request)
        {
            await Task.Delay(request.RequestTimeout);
            if (!request.RequestAccepted)
            {
                request.TimedOut = true;
                request.DateTimeWhenProcessed = DateTime.Now;
                _db.UpdateMentorRequestDb(request);
                _requestsBackplane.RemoveActiveRequest(request);
                NotifyMentorOfRequestTimeout(request.Mentor);
            }
        }

        private async Task StartMentorReservationTimeoutAsync(MentorRequest request)
        {
            if (request.MentoringDuration != null)
            {
                await Task.Delay(request.MentoringDuration);
                if (_requestsBackplane.ContainsOutstandingRequestForMentor(request.Mentor.Id))
                {
                    var mentor = request.Mentor;
                    mentor.IsAvailable = true;
                    _db.UpdateMentorDb(mentor);
                    NotifyResponseTimeUp(request);
                }
            }
        }

        #endregion

        #region ResponseHandlers

        private void HandleResponseWithNoRequest(SmsDto inboundSms)
        {
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            ResponseUnIdentified(inboundSms);
            _db.UpdateSmsDb(inboundSms);
        }

        private void HandleUnidentifiedRequestResponse(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            _db.UpdateSmsDb(inboundSms);
        }

        private void HandleRequestRejection(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            mentorRequest.RequestAccepted = false;
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.InboundSms = inboundSms;

            var mentor = mentorRequest.Mentor;
            mentor.IsAvailable = true;
            _db.UpdateMentorDb(mentor);

            _db.UpdateMentorRequestDb(mentorRequest);
            _db.UpdateSmsDb(inboundSms);

            _Notifier.UpdateMentorRequestee(mentorRequest);
        }

        private void HandleRequestAcceptance(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            mentorRequest.RequestAccepted = true;
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.InboundSms = inboundSms;
            mentorRequest.Mentor.IsAvailable = false;

            _db.UpdateSmsDb(inboundSms);
            _db.UpdateMentorDb(mentorRequest.Mentor);
            _db.UpdateMentorRequestDb(mentorRequest);

            _Notifier.UpdateMentorRequestee(mentorRequest);
        }

        private void HandleGuideResponse(SmsDto inboundSms)
        {
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            _db.UpdateSmsDb(inboundSms);
        }

        private void HandleBusyResponse(Mentor mentor, SmsDto sms)
        {
            mentor.IsAvailable = false;
            _db.UpdateMentorDb(mentor);
            sms.DateTimeWhenProcessed = DateTime.Now;
            _db.UpdateSmsDb(sms);
        }

        private void HandleAvailableResponse(Mentor mentor, SmsDto inboundSms)
        {
            if (_requestsBackplane.ContainsOutstandingRequestForMentor(mentor.Id))
                _requestsBackplane.RemoveActiveRequestByMentorId(mentor.Id);
            mentor.IsAvailable = true;
            _db.UpdateMentorDb(mentor);
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            _db.UpdateSmsDb(inboundSms);
        }

        #endregion

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

        private bool IsGuideResponse(SmsDto inboundSms)
        {
            if (inboundSms.MessageBody.Trim().ToLower() == "guide")
                return true;

            return false;
        }

        private bool IsBusyResponse(SmsDto inboundSms)
        {
            if (inboundSms.MessageBody.Trim().ToLower() == "busy")
                return true;

            return false;
        }

        private bool IsAvailableResponse(SmsDto inboundSms)
        {
            if (inboundSms.MessageBody.Trim().ToLower() == "available")
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

        private void ResponseUnIdentified(SmsDto smsResponse, SmsDto lastSmsSent = null)
        {
            string message = $"Uncertain how to execute your objective.";
            _sms.SendSms(smsResponse.FromPhoneNumber, message);
            
            //Will not currently be null because there's nothing being passed in as the lastSmsSent param
            //RESEND THE INITAL PROMPT SMS
            if (lastSmsSent != null)
                _sms.SendSms(smsResponse.FromPhoneNumber, lastSmsSent.MessageBody);
            else
                ResponseProcessedGuide(smsResponse);
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
                "Please notify the responsable party if you would like to be marked as unavailable or busy.");
        }

        private void ResponseProcessedGuide(SmsDto inboundSms)
        {
            _sms.SendSms(inboundSms.FromPhoneNumber,
                "You may enter the following at any time." + Environment.NewLine +
                "Guide: Reads out valid responses" + Environment.NewLine +
                "Busy: Sets you as unavailable" + Environment.NewLine + 
                "Available: Sets you as available");
        }

        private void ResponseProcessedBusy(SmsDto inboundSms)
        {
            _sms.SendSms(inboundSms.FromPhoneNumber,
                "Response recieved and you've been marked as unavailable.");
        }

        private void ResponseProcessedAvailable(SmsDto inboundSms)
        {
            _sms.SendSms(inboundSms.FromPhoneNumber,
                "Response recieved and you've been marked as available again.");
        }

        #endregion MessageResponseHelperMethods
    }
}