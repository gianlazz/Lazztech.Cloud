using Lazztech.Events.Dal;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.EntityFrameworkCore;
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
            _context.Entry(requestEntity).State = EntityState.Detached;
            request.Id = requestEntity.MentorRequestId;
        }

        public void AddSmsDb(ref SmsDto inboundSms)
        {
            var smsEntity = inboundSms.MapToEntity();
            _context.SmsMessages.Add(smsEntity);
            _context.SaveChanges();
            _context.Entry(smsEntity).State = EntityState.Detached;
            inboundSms.Id = smsEntity.SmsId;
        }

        public Mentor FindMentor(int Id)
        {
            var entity = _context.Mentors
                .AsNoTracking()
                .Include(x => x.Event)
                .ThenInclude(x => x.Organization)
                .FirstOrDefault(x => x.MentorId == Id);
            var dto = entity.MapToDto();
            return dto;
        }

        public Mentor FindMentorByPhoneNumber(string phoneNumber)
        {
            var entity = _context.Mentors
                .AsNoTracking()
                .Include(x => x.Event)
                .ThenInclude(x => x.Organization)
                .FirstOrDefault(x => x.PhoneNumber.Contains(phoneNumber.TrimStart("+1".ToCharArray())));
            var dto = entity.MapToDto();
            return dto;
        }

        public void UpdateMentorDb(Mentor mentor)
        {
            var entity = mentor.MapToEntity();
            _context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.Entry(entity).Reference(x => x.Event).IsModified = false;
            //_context.Entry(entity).Property(x => x.Event).IsModified = false;

            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void UpdateMentorRequestDb(MentorRequest request)
        {
            var entity = request.MapToEntity();
            _context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Attach(entity.Mentor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.Entry(entity.Mentor).Reference(x => x.Event).IsModified = false;
            //_context.Entry(entity).Property(x => x.Mentor.Event).IsModified = false;

            if (entity.OutboundSms != null)
                _context.Attach(entity.OutboundSms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            if (entity.InboundSms != null)
                _context.Attach(entity.InboundSms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
            _context.Entry(entity.Mentor).State = EntityState.Detached;
            if (entity.OutboundSms != null)
                _context.Entry(entity.OutboundSms).State = EntityState.Detached;
            if (entity.InboundSms != null)
                _context.Entry(entity.InboundSms).State = EntityState.Detached;
        }

        public void UpdateSmsDb(SmsDto inboundSms)
        {
            var entity = inboundSms.MapToEntity();
            _context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(entity);
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
