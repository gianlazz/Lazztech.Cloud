using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dal
{
    public static class EntityDtoMapperExtensions
    {
        #region MentorRequest
        public static Dao.MentorRequest MapToEntity(this Events.Dto.Models.MentorRequest dto)
        {
            if (dto != null)
            {
                var request = new Dao.MentorRequest()
                {
                    MentorRequestId = dto.Id,
                    IsStillActive = dto.IsStillActive,
                    DateTimeOfRequest = dto.DateTimeOfRequest,
                    MentorId = dto.Mentor.Id,
                    Mentor = dto.Mentor.MapToEntity(),
                    UniqueRequesteeId = dto.UniqueRequesteeId,
                    RequestAccepted = dto.RequestAccepted,
                    DateTimeWhenProcessed = dto.DateTimeWhenProcessed,
                    OutboundSms = dto.OutboundSms.MapToEntity(),
                    InboundSms = dto.InboundSms.MapToEntity(),
                    MentoringDuration = dto.MentoringDuration,
                    RequestTimeout = dto.RequestTimeout,
                    TimedOut = dto.TimedOut
                };
                return request;
            }
            else
                return null;
        }

        public static Dto.Models.MentorRequest MapToDto(this Dao.MentorRequest entity)
        {
            if (entity != null)
            {
                var request = new Dto.Models.MentorRequest()
                {
                    Id = entity.MentorRequestId,
                    IsStillActive = entity.IsStillActive,
                    DateTimeOfRequest = entity.DateTimeOfRequest,
                    Mentor = entity.Mentor.MapToDto(),
                    UniqueRequesteeId = entity.UniqueRequesteeId,
                    RequestAccepted = entity.RequestAccepted,
                    DateTimeWhenProcessed = entity.DateTimeWhenProcessed,
                    OutboundSms = entity.OutboundSms.MapToDto(),
                    InboundSms = entity.InboundSms.MapToDto(),
                    MentoringDuration = entity.MentoringDuration,
                    RequestTimeout = entity.RequestTimeout,
                    TimedOut = entity.TimedOut
                };
                return request;
            }
            else
                return null;
        }
        #endregion

        #region Mentor
        public static Dao.Mentor MapToEntity(this Dto.Models.Mentor dto)
        {
            if (dto != null)
            {
                var mentor = new Dao.Mentor()
                {
                    MentorId = dto.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Image = dto.Image,
                    Skills = dto.Skills,
                    ProfessionalTitle = dto.ProfessionalTitle,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    IsAvailable = dto.IsAvailable,
                    IsPresent = dto.IsPresent
                };
                return mentor;
            }
            else
                return null;
        }

        public static Dto.Models.Mentor MapToDto(this Dao.Mentor entity)
        {
            if (entity != null)
            {
                var mentor = new Dto.Models.Mentor()
                {
                    Id = entity.MentorId,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Image = entity.Image,
                    Skills = entity.Skills,
                    ProfessionalTitle = entity.ProfessionalTitle,
                    PhoneNumber = entity.PhoneNumber,
                    Email = entity.Email,
                    IsAvailable = entity.IsAvailable,
                    IsPresent = entity.IsPresent
                };
                return mentor;
            }
            else
                return null;
        }
        #endregion

        #region Sms
        public static Sms MapToEntity(this SmsDto smsDto)
        {
            if (smsDto != null)
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
            else
                return null;
        }

        public static SmsDto MapToDto(this Sms smsEntity)
        {
            if (smsEntity != null)
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
            else
                return null;
        }
        #endregion
    }
}
