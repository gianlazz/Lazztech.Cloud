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
        public ConcurrentBag<SmsDto> InboundMessages { get; private set; }
        public Dictionary<string, MentorRequest> Requests { get; private set; }

        private readonly IRepository _db;
        private readonly ISmsService _sms;
        private readonly IRequestResponder _recResponder;

        public MentorRequestConductor(IRepository repository, ISmsService sms, IRequestResponder requestResponder)
        {
            _db = repository;
            _sms = sms;
            _recResponder = requestResponder;
            InboundMessages = new ConcurrentBag<SmsDto>();
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
                //_db.Add<MentorRequest>(request);
                return true;
            }
        }

        public void ProcessInboundSms(SmsDto inboundSms)
        {
            //_db.Add<SmsDto>(inboundSms);
            foreach (var mentorRequest in Requests.Values.Where(x => x.DateTimeWhenProcessed == null && x.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber))
            {
                if (IsAcceptanceResponse(inboundSms))
                {
                    HandleRequestAcceptance(inboundSms, mentorRequest);
                    AcceptanceResponseConfirmation(inboundSms, mentorRequest);
                    ResponseProcessedConfirmation(inboundSms);
                    StartMentorReservationTimeoutAsync(mentorRequest);
                }
                else if (IsRejectionResponse(inboundSms))
                {
                    ResponseProcessedConfirmation(inboundSms);
                    HandleRequestRejection(inboundSms, mentorRequest);
                }
                else
                    HandleUnidentifiedRequestResponse(inboundSms, mentorRequest);
            }

            if (inboundSms.DateTimeWhenProcessed == null)
                HandleResponseWithNoRequest(inboundSms);
        }

        private async Task StartMentorReservationTimeoutAsync(MentorRequest request)
        {
            if (request.Timeout != null)
            {
                await Task.Delay(request.Timeout);
                request.Mentor.IsAvailable = true;
                _db.Delete(request.Mentor);
                _db.Add(request.Mentor);
                _sms.SendSms(request.Mentor.PhoneNumber,
                    "Your reserved time is up and you've been marked as available. " +
                    "You may continue helping person(s) though until you've recieved another request.");
            }
        }

        private void HandleResponseWithNoRequest(SmsDto inboundSms)
        {
            _db.Delete<SmsDto>(inboundSms);
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            UnIdentifiedResponse(inboundSms);
            _db.Add<SmsDto>(inboundSms);
        }

        private void HandleUnidentifiedRequestResponse(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            _db.Delete<SmsDto>(inboundSms);
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            UnIdentifiedResponse(inboundSms, mentorRequest.OutboundSms);
            _db.Add<SmsDto>(inboundSms);
        }

        private void HandleRequestRejection(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            _db.Delete<MentorRequest>(mentorRequest);
            _db.Delete<SmsDto>(inboundSms);
            mentorRequest.RequestAccepted = false;
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.InboundSms = inboundSms;
            _db.Add<MentorRequest>(mentorRequest);
            _db.Add<SmsDto>(inboundSms);
            //follow-up steps:
            //NOTIFY SIGNALR TEAM
            _recResponder.MentorRequestResponse(mentorRequest);
        }

        private void HandleRequestAcceptance(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            _db.Delete<MentorRequest>(mentorRequest);
            _db.Delete<SmsDto>(inboundSms);
            mentorRequest.RequestAccepted = true;
            inboundSms.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.DateTimeWhenProcessed = DateTime.Now;
            mentorRequest.InboundSms = inboundSms;
            _db.Add<MentorRequest>(mentorRequest);
            _db.Add<SmsDto>(inboundSms);
            //follow-up steps:
            _db.Delete<Mentor>(mentorRequest.Mentor);
            mentorRequest.Mentor.IsAvailable = false;
            _db.Add<Mentor>(mentorRequest.Mentor);
            //NOTIFY SIGNALR TEAM
            //_recResponder.MentorRequestResponse(mentorRequest);
        }

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

        private void AcceptanceResponseConfirmation(SmsDto inboundSms, MentorRequest mentorRequest)
        {
            string message = $"Response confirmed. " +
                $"You've been marked as busy for {mentorRequest.Timeout.Minutes} minutes" +
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
            //SHOULD IT BE ADDED TO THE OUTBOUND MESSAGES?
        }
    }
}