using System.Collections.Generic;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IMentorRequestConductor
    {
        MentorRequest ProcessResponse(SmsDto inboundSms);
        void SubmitRequest(string uniqueRequesteeId, string teamName, string teamLocation, int mentorId);
    }
}