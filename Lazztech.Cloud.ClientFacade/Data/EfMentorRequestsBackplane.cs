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
        private readonly ApplicationDbContext _context;

        public EfMentorRequestsBackplane(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void AddMentorRequest(ref MentorRequest mentorRequest)
        {
            var entity = mentorRequest.MapToEntity();
            _context.Add(entity);
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
            mentorRequest.Id = entity.MentorId;
        }

        public bool ContainsOutstandingRequestForMentor(int mentorId)
        {
            return _context.MentorRequests.AsNoTracking()
                .Any(x => x.MentorId == mentorId
                &&
                x.IsStillActive == true);
        }

        public MentorRequest FindResponseRequest(SmsDto inboundSms)
        {
            var matchingRequestEntity = _context.MentorRequests
                .AsNoTracking()
                .Include(x => x.Mentor)
                .ThenInclude(x => x.Event)
                .ThenInclude(x => x.Organization)
                .FirstOrDefault(x => x.DateTimeWhenProcessed == null
                &&
                x.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber);
            if (matchingRequestEntity != null)
                _context.Entry(matchingRequestEntity).State = EntityState.Detached;
            var dto = matchingRequestEntity.MapToDto();
            return dto;
        }

        public void RemoveActiveRequest(MentorRequest request)
        {
            var entity = _context.MentorRequests.FirstOrDefault(x => x.MentorId == request.Id);
            entity.IsStillActive = false;
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void RemoveActiveRequestByMentorId(int mentorId)
        {
            var entity = _context.MentorRequests
                .FirstOrDefault(x => x.IsStillActive && x.MentorId == mentorId);
            entity.IsStillActive = false;
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
