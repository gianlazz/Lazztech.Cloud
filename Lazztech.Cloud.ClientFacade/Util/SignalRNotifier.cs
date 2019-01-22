using Lazztech.Cloud.ClientFacade.Hubs;
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

        public void UpdateMentorRequestee(MentorRequest mentorRequest)
        {
            if (mentorRequest.RequestAccepted == true && mentorRequest.DateTimeWhenProcessed != null)
            {
                if (mentorRequest.UniqueRequesteeId == null) { throw new Exception(); }


                var percentage = (20 * 100) / 100;
                var message = $"{mentorRequest.Mentor.FirstName} accepted your request!";
                //PUSHING DATA TO ALL CLIENTS
                _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("requestUpdate", message, percentage);

                //var hub = new ProgressHub();
                //hub.UpdateTeamOfMentorRequest(mentorRequest.UniqueRequesteeId, true, message);
            }
            if (mentorRequest.RequestAccepted == false && mentorRequest.DateTimeWhenProcessed != null)
            {
                if (mentorRequest.UniqueRequesteeId == null) { throw new Exception(); }


                var percentage = (20 * 100) / 100;
                var message = $"{mentorRequest.Mentor.FirstName} is not available right now";
                _hubContext.Clients.Group(mentorRequest.UniqueRequesteeId).SendAsync("requestUpdate", message, percentage);

                //var hub = new ProgressHub();
                //hub.UpdateTeamOfMentorRequest(mentorRequest.UniqueRequesteeId, true, message);
            }
        }
    }
}