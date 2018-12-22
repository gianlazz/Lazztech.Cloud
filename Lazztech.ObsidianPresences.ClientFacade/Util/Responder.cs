﻿using HackathonManager.Models;
using HackathonManager.Sms;
using Lazztech.ObsidianPresences.ClientFacade.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.ObsidianPresences.ClientFacade.Util
{
    public class Responder : IRequestResponder
    {
        public void MentorRequestResponse(MentorRequest mentorRequest)
        {
            if (mentorRequest.RequestAccepted == true && mentorRequest.DateTimeWhenProcessed != null)
            {
                var message = $"{mentorRequest.Mentor.FirstName} accepted your request!";
                ProgressHub.UpdateTeamOfMentorRequest(mentorRequest.TeamName, true, message);
            }
            if (mentorRequest.RequestAccepted == false && mentorRequest.DateTimeWhenProcessed != null)
            {
                var message = $"{mentorRequest.Mentor.FirstName} is not available right now";
                ProgressHub.UpdateTeamOfMentorRequest(mentorRequest.TeamName, true, message);
            }
        }
    }
}