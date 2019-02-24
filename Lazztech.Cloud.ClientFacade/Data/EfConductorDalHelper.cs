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
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public EfConductorDalHelper(ApplicationDbContext applicationDbContext, DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public void AddMenorRequestDb(ref MentorRequest request)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var requestEntity = request.MapToEntity();
                context.MentorRequests.Add(requestEntity);
                context.SaveChanges();
                context.Entry(requestEntity).State = EntityState.Detached;
                request.Id = requestEntity.MentorRequestId;
            }
        }

        public void AddSmsDb(ref SmsDto inboundSms)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var smsEntity = inboundSms.MapToEntity();
                context.SmsMessages.Add(smsEntity);
                context.SaveChanges();
                context.Entry(smsEntity).State = EntityState.Detached;
                inboundSms.Id = smsEntity.SmsId;
            }
        }

        public Mentor FindMentor(int Id)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = context.Mentors
                        .AsNoTracking()
                        .Include(x => x.Event)
                        .ThenInclude(x => x.Organization)
                        .FirstOrDefault(x => x.MentorId == Id);
                var dto = entity.MapToDto();
                return dto;
            }
        }

        public Mentor FindMentorByPhoneNumber(string phoneNumber)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = context.Mentors
                    .AsNoTracking()
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Organization)
                    .FirstOrDefault(x => x.PhoneNumber.Contains(phoneNumber.TrimStart("+1".ToCharArray())));
                var dto = entity.MapToDto();
                return dto;
            }
        }

        public MentorRequest FindMentorRequestById(int id)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var matchingRequestEntity = context.MentorRequests
                    .AsNoTracking()
                    .Include(x => x.Mentor)
                    .ThenInclude(x => x.Event)
                    .ThenInclude(x => x.Organization)
                    .FirstOrDefault(x => x.MentorRequestId == id);

                var dto = matchingRequestEntity.MapToDto();
                return dto;
            }
        }

        public void UpdateMentorDb(Mentor mentor)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = mentor.MapToEntity();
                context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.Entry(entity).Reference(x => x.Event).IsModified = false;
                //_context.Entry(entity).Property(x => x.Event).IsModified = false;

                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
            }
        }

        public void UpdateMentorRequestDb(MentorRequest request)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = request.MapToEntity();
                context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.Attach(entity.Mentor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.Entry(entity.Mentor).Reference(x => x.Event).IsModified = false;
                //_context.Entry(entity).Property(x => x.Mentor.Event).IsModified = false;

                if (entity.OutboundSms != null)
                    context.Attach(entity.OutboundSms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                if (entity.InboundSms != null)
                    context.Attach(entity.InboundSms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
                context.Entry(entity.Mentor).State = EntityState.Detached;
                if (entity.OutboundSms != null)
                    context.Entry(entity.OutboundSms).State = EntityState.Detached;
                if (entity.InboundSms != null)
                    context.Entry(entity.InboundSms).State = EntityState.Detached;
            }
        }

        public async Task UpdateMentorRequestDbAsync(MentorRequest request)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = request.MapToEntity();
                context.Update(entity);
                //_context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //_context.Attach(entity.Mentor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                context.Entry(entity.Mentor).Reference(x => x.Event).IsModified = false;
                //_context.Entry(entity).Property(x => x.Mentor.Event).IsModified = false;

                if (entity.OutboundSms != null)
                    context.Attach(entity.OutboundSms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                if (entity.InboundSms != null)
                    context.Attach(entity.InboundSms).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await context.SaveChangesAsync();
                context.Entry(entity).State = EntityState.Detached;
                context.Entry(entity.Mentor).State = EntityState.Detached;
                if (entity.OutboundSms != null)
                    context.Entry(entity.OutboundSms).State = EntityState.Detached;
                if (entity.InboundSms != null)
                    context.Entry(entity.InboundSms).State = EntityState.Detached;
            }
        }

        public void UpdateSmsDb(SmsDto inboundSms)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = inboundSms.MapToEntity();
                context.Attach(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.Update(entity);
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
