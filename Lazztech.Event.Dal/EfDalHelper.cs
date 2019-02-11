using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lazztech.Event.Dal
{
    public class EfDalHelper : IDalHelper
    {
        private readonly DbContext _db;

        public EfDalHelper(DbContext dbContext)
        {
            _db = dbContext;
        }

        public void Add<T>(T item) where T : class, new()
        {
            _db.Add(item);
            _db.SaveChanges();
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            _db.AddRange(items);
            _db.SaveChanges();
        }

        public void AddMenorRequestDb(MentorRequest request)
        {
            throw new NotImplementedException();
        }

        public void AddSmsDb(SmsDto inboundSms)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T item) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void UpdateMentorDb(Mentor mentor)
        {
            throw new NotImplementedException();
        }

        public void UpdateMentoRequestDb(MentorRequest request)
        {
            throw new NotImplementedException();
        }

        public void UpdateSmsDb(SmsDto inboundSms)
        {
            throw new NotImplementedException();
        }
    }
}
