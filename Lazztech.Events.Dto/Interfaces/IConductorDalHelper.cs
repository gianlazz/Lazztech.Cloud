using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IConductorDalHelper
    {
        void AddMenorRequestDb(ref MentorRequest request);
        void AddSmsDb(ref SmsDto inboundSms);
        void UpdateMentorRequestDb(MentorRequest request);
        Task UpdateMentorRequestDbAsync(MentorRequest request);
        MentorRequest FindMentorRequestById(int id);
        void UpdateMentorDb(Mentor mentor);
        void UpdateSmsDb(SmsDto inboundSms);
        Dto.Models.Mentor FindMentor(int Id);
        Dto.Models.Mentor FindMentorByPhoneNumber(string phoneNumber);
    }
}
