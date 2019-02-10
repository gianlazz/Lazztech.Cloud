using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Lazztech.MongoDB
{
    public class MongoDalHelper : IDalHelper
    {
        private readonly IRepository _db;

        public MongoDalHelper(IRepository repository)
        {
            _db = repository;
        }

        public void AddSmsDb(SmsDto inboundSms)
        {
            _db.Add<SmsDto>(inboundSms);
        }

        public void AddMenorRequestDb(MentorRequest request)
        {
            _db.Add<MentorRequest>(request);
        }

        public void UpdateMentoRequestDb(MentorRequest request)
        {
            _db.Delete<MentorRequest>(x => x.Id == request.Id);
            _db.Add<MentorRequest>(request);
        }

        public void UpdateMentorDb(Mentor mentor)
        {
            _db.Delete<Mentor>(x => x.Id == mentor.Id);
            _db.Add<Mentor>(mentor);
        }

        public void UpdateSmsDb(SmsDto inboundSms)
        {
            _db.Delete<SmsDto>(x => x.GuidId == inboundSms.GuidId);
            _db.Add<SmsDto>(inboundSms);
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return _db.Single(expression);
        }
    }
}
