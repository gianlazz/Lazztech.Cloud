using System;
using System.Collections.Generic;
using System.Text;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IConductorDalHelper
    {
        void AddMenorRequestDb(ref MentorRequest request);
        void AddSmsDb(ref SmsDto inboundSms);
        void UpdateMentoRequestDb(MentorRequest request);
        void UpdateMentorDb(Mentor mentor);
        void UpdateSmsDb(SmsDto inboundSms);
        Dto.Models.Mentor FindMentor(int Id);
        Dto.Models.Mentor FindMentorByPhoneNumber(string phoneNumber);
    }
}
