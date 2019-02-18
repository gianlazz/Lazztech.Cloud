using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IMentorRequestsBackplane
    {
        bool ContainsOutstandingRequestForMentor(string phoneNumber);
        void AddMentorRequest(Events.Dto.Models.MentorRequest mentorRequest);
        Models.MentorRequest FindResponseRequest(Dto.Models.SmsDto inboundSms);
    }
}
