﻿using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IDalHelper
    {
        void AddSmsDb(SmsDto inboundSms);
        void AddMenorRequestDb(MentorRequest request);
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void UpdateMentoRequestDb(MentorRequest request);
        void UpdateMentorDb(Mentor mentor);
        void UpdateSmsDb(SmsDto inboundSms);
    }
}
