﻿using Lazztech.Cloud.ClientFacade.Hubs;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.SignalR;
using System;

namespace Lazztech.Cloud.ClientFacade.Util
{
    public class SignalRNotifier : IRequestNotifier
    {
        private readonly IHubContext<ProgressHub> _hubContext;

        public SignalRNotifier(IHubContext<ProgressHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void NofityThatMentorIsntAvailable(string uniqueRequesteeId, string mentorName)
        {
            if (String.IsNullOrEmpty(uniqueRequesteeId)) { throw new Exception(); }

            var message = $"{mentorName} isn't available for a new request. Please refresh your page.";
            _hubContext.Clients.Group(uniqueRequesteeId).SendAsync("requestUpdate", message);
        }

        public void NotifyThatRequestHasBeenSent(MentorRequest mentorRequest)
        {
            _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("confetti");
            _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("notifySuccessAutoHide", "Request Sent", "Please wait for their response.");
        }

        public void NofityThatMentorDidntRespond(MentorRequest request)
        {
            var message = $"{request.Mentor.FirstName} didn't reply before the request timout.";
            _hubContext.Clients.Group(request.UniqueRequesteeId).SendAsync("notifyError", "Request Timed Out", message);
        }

        public void NofityThatMentorAvailableAgain(string mentorName)
        {
            var message = $"{mentorName} has been set as available again. Please refresh your page.";
            _hubContext.Clients.All.SendAsync("requestUpdate", message);
        }

        public void UpdateMentorRequestee(MentorRequest mentorRequest)
        {
            if (mentorRequest.RequestAccepted == true && mentorRequest.DateTimeWhenProcessed != null)
            {
                if (mentorRequest.UniqueRequesteeId == null) { throw new Exception(); }

                var message = $"{mentorRequest.Mentor.FirstName} accepted your request! They'll be on their way.";
                _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("confetti");
                _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("notifySuccess", "Request Accepted", message);
            }
            if (mentorRequest.RequestAccepted == false && mentorRequest.DateTimeWhenProcessed != null)
            {
                if (mentorRequest.UniqueRequesteeId == null) { throw new Exception(); }

                var message = $"{mentorRequest.Mentor.FirstName} denied request.";
                _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("notifyError", "Request Denied", message);
            }
        }
    }
}