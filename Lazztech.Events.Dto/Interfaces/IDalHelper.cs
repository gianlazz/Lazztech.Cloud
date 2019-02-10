using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IDalHelper
    {
        void AddSmsDb(SmsDto inboundSms);
        void AddMenorRequestDb(MentorRequest request);
        void UpdateMentoRequestDb(MentorRequest request);
        void UpdateMentorDb(Mentor mentor);
        void UpdateSmsDb(SmsDto inboundSms);
    }
}
