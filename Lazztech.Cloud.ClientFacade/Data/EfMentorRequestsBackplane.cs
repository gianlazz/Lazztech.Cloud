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
    public class EfMentorRequestsBackplane : IMentorRequestsBackplane
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public EfMentorRequestsBackplane(ApplicationDbContext applicationDbContext, DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public void AddMentorRequest(ref MentorRequest mentorRequest)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = mentorRequest.MapToEntity();
                context.Add(entity);
                context.Entry(entity.Mentor).State = EntityState.Detached;
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
                if (entity.OutboundSms != null)
                    context.Entry(entity.OutboundSms).State = EntityState.Detached;
                if (entity.InboundSms != null)
                    context.Entry(entity.InboundSms).State = EntityState.Detached;
                mentorRequest.Id = entity.MentorRequestId;
            }
        }

        public bool ContainsOutstandingRequestForMentor(int mentorId)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var result = context.MentorRequests.AsNoTracking().Any(x => x.MentorId == mentorId && x.IsStillActive == true);
                return result;
            }
        }

        public MentorRequest FindResponseRequest(SmsDto inboundSms)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var matchingRequestEntity = context.MentorRequests
                        .AsNoTracking()
                        .Include(x => x.Mentor)
                        .ThenInclude(x => x.Event)
                        .ThenInclude(x => x.Organization)
                        .FirstOrDefault(x => x.DateTimeWhenProcessed == null
                        &&
                        x.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber);

                if (matchingRequestEntity != null)
                    context.Entry(matchingRequestEntity).State = EntityState.Detached;
                var dto = matchingRequestEntity.MapToDto();
                return dto;
            }
        }

        public void RemoveActiveRequest(MentorRequest request)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = context.MentorRequests.FirstOrDefault(x => x.MentorId == request.Id);
                entity.IsStillActive = false;
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
            }
        }

        public void RemoveActiveRequestByMentorId(int mentorId)
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var entity = context.MentorRequests
                .FirstOrDefault(x => x.IsStillActive && x.MentorId == mentorId);
                entity.IsStillActive = false;
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
