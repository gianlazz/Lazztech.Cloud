using Lazztech.Events.Dal;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Data
{
    public class EfConductorDalHelper : IConductorDalHelper
    {
        private readonly ApplicationDbContext _context;

        public EfConductorDalHelper(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void AddMenorRequestDb(ref MentorRequest request)
        {
            var requestEntity = request.MapToEntity();
            _context.MentorRequests.Add(requestEntity);
            _context.SaveChanges();
            request.Id = requestEntity.MentorRequestId;
        }

        public void AddSmsDb(ref SmsDto inboundSms)
        {
            var smsEntity = inboundSms.MapToEntity();
            _context.SmsMessages.Add(smsEntity);
            _context.SaveChanges();
            inboundSms.Id = smsEntity.SmsId;
        }

        public Mentor FindMentor(int Id)
        {
            var entity = _context.Mentors.FirstOrDefault(x => x.MentorId == Id);
            var dto = entity.MapToDto();
            return dto;
        }

        public Mentor FindMentorByPhoneNumber(string phoneNumber)
        {
            var entity = _context.Mentors.FirstOrDefault(x => x.PhoneNumber.Contains(phoneNumber.TrimStart("+1".ToCharArray())));
            var dto = entity.MapToDto();
            return dto;
        }

        public void UpdateMentorDb(Mentor mentor)
        {
            var entity = mentor.MapToEntity();
            _context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdateMentoRequestDb(MentorRequest request)
        {
            var entity = request.MapToEntity();
            _context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdateSmsDb(SmsDto inboundSms)
        {
            var entity = inboundSms.MapToEntity();
            _context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
