using Lazztech.Cloud.ClientFacade.Hubs;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Cloud.ClientFacade.Util
{
    public class Responder : IRequestResponder
    {
        public void MentorRequestResponse(MentorRequest mentorRequest)
        {
            if (mentorRequest.RequestAccepted == true && mentorRequest.DateTimeWhenProcessed != null)
            {
                var message = $"{mentorRequest.Mentor.FirstName} accepted your request!";
                var hub = new ProgressHub();
                hub.UpdateTeamOfMentorRequest(mentorRequest.UniqueRequesteeId, true, message);
            }
            if (mentorRequest.RequestAccepted == false && mentorRequest.DateTimeWhenProcessed != null)
            {
                var message = $"{mentorRequest.Mentor.FirstName} is not available right now";
                var hub = new ProgressHub();
                hub.UpdateTeamOfMentorRequest(mentorRequest.UniqueRequesteeId, true, message);
            }
        }
    }
}