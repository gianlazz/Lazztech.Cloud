﻿using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Lazztech.Events.Domain.Sms
{
    public class SmsRoutingConductor
    {
        public static ConcurrentBag<SmsDto> InboundMessages = new ConcurrentBag<SmsDto>();
        public static ConcurrentBag<MentorRequest> MentorRequests = new ConcurrentBag<MentorRequest>();

        private readonly IRepository _db;
        private readonly ISmsService _sms;
        private readonly IRequestResponder _recResponder;

        public SmsRoutingConductor(IRepository repository, ISmsService sms, IRequestResponder requestResponder)
        {
            _db = repository;
            _sms = sms;
            _recResponder = requestResponder;
        }

        public void ProcessMentorRequests()
        {
            foreach (var inboundSms in InboundMessages.Where(x => x.DateTimeWhenProcessed == null))
            {
                foreach (var mentorRequest in MentorRequests.Where(x => x.DateTimeWhenProcessed == null))
                {
                    if (mentorRequest.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber)
                    {
                        if (IsAcceptanceResponse(inboundSms))
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
                            ResponseProcessedConfirmation(inboundSms);
                            _db.Delete<Mentor>(mentorRequest.Mentor);
                            mentorRequest.Mentor.IsAvailable = false;
                            _db.Add<Mentor>(mentorRequest.Mentor);
                            //NOTIFY SIGNALR TEAM
                            _recResponder.MentorRequestResponse(mentorRequest);
                        }
                        else if (IsRejectionResponse(inboundSms))
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
                            ResponseProcessedConfirmation(inboundSms);
                            //NOTIFY SIGNALR TEAM
                            _recResponder.MentorRequestResponse(mentorRequest);
                        }
                        else
                        {
                            _db.Delete<SmsDto>(inboundSms);
                            inboundSms.DateTimeWhenProcessed = DateTime.Now;
                            UnIdentifiedResponse(inboundSms, mentorRequest.OutboundSms);
                            _db.Add<SmsDto>(inboundSms);
                        }
                    }
                }

                if (inboundSms.DateTimeWhenProcessed == null)
                {
                    _db.Delete<SmsDto>(inboundSms);
                    inboundSms.DateTimeWhenProcessed = DateTime.Now;
                    UnIdentifiedResponse(inboundSms);
                    _db.Add<SmsDto>(inboundSms);
                }
            }
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