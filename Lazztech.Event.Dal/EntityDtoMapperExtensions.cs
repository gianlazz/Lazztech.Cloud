using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dal
{
    public static class EntityDtoMapperExtensions
    {
        public static Sms MapToSmsEntity(this SmsDto smsDto)
        {
            var sms = new Sms()
            {
                SmsId = smsDto.Id,
                DateCreated = smsDto.DateCreated,
                ToPhoneNumber = smsDto.ToPhoneNumber,
                FromPhoneNumber = smsDto.FromPhoneNumber,
                MessageBody = smsDto.MessageBody,
                Sid = smsDto.Sid,
                DateTimeWhenProcessed = smsDto.DateTimeWhenProcessed
            };
            return sms;
        }

        public static SmsDto MapToSmsDto(this Sms smsEntity)
        {
            var sms = new SmsDto()
            {
                Id = smsEntity.SmsId,
                DateCreated = smsEntity.DateCreated,
                ToPhoneNumber = smsEntity.ToPhoneNumber,
                FromPhoneNumber = smsEntity.FromPhoneNumber,
                MessageBody = smsEntity.MessageBody,
                Sid = smsEntity.Sid,
                DateTimeWhenProcessed = smsEntity.DateTimeWhenProcessed
            };

            return sms;
        }
    }
}
