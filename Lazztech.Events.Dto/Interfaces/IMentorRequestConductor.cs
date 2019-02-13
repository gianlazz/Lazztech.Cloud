using System.Collections.Generic;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IMentorRequestConductor
    {
        Dictionary<string, MentorRequest> Requests { get; }

        MentorRequest ProcessResponse(SmsDto inboundSms);
        bool TryAddRequest(MentorRequest request);
    }
}