using Lazztech.Events.Dal;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
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

        public void AddMentorRequest(MentorRequest mentorRequest)
        {
            var entity = mentorRequest.MapToEntity();
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool ContainsOutstandingRequestForMentor(string phoneNumber)
        {
            return _context.MentorRequests
                .Any(x => x.Mentor.PhoneNumber
                .TrimStart("+1".ToArray())
                .Contains(phoneNumber.TrimStart("+1".ToArray())));
        }

        public MentorRequest FindResponseRequest(SmsDto inboundSms)
        {
            var matchingRequestEntity = _context.MentorRequests
                .FirstOrDefault(x => x.DateTimeWhenProcessed == null
                &&
                x.OutboundSms.ToPhoneNumber == inboundSms.FromPhoneNumber);

            var dto = matchingRequestEntity.MapToDto();
            return dto;
        }
    }
}
