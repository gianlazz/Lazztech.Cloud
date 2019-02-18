using System;
using System.Collections.Generic;
using System.Text;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IMentorRequestsBackplane
    {
        bool ContainsOutstandingRequestForMentor(int mentorId);
        void AddMentorRequest(Events.Dto.Models.MentorRequest mentorRequest);
        Models.MentorRequest FindResponseRequest(Dto.Models.SmsDto inboundSms);
        void RemoveActiveRequest(MentorRequest request);
        void RemoveActiveRequestByMentorId(int mentorId);
    }
}
