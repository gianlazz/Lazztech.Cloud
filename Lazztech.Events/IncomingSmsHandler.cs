using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackathonManager.DTO;
using HackathonManager.Interfaces;
using HackathonManager.PocoModels;
using HackathonManager.RepositoryPattern;

namespace HackathonManager
{
    public class IncomingSmsHandler
    {
        private ISmsService _smsService;
        private IRepository _repo;
        private ILogger _logger;

        public IncomingSmsHandler(ISmsService smsService, IRepository repository, ILogger logger)
        {
            _smsService = smsService;
            _repo = repository;
            _logger = logger;
        }

        //public void Process(SmsDto incomingSmsDto)
        //{
        //    if (CheckIfTheyreValidUser(incomingSmsDto))
        //        return;
        //    Isfinished(incomingSmsDto);
        //}

        private bool Isfinished(SmsDto incomingSmsDto)
        {
            if (incomingSmsDto.MessageBody.ToLower().Contains("finished"))
            {
                _smsService.SendSms(incomingSmsDto.FromPhoneNumber,
                    "Thank you, you'll now be set back as available for others" +
                    "to request your help.");

                //Set the person from the incomingSmsDto back to isAvailable = true
                return true;
            }
            else
            {
                return false;
            }
        }

        //private bool CheckIfTheyreValidUser(SmsDto incomingSms)
        //{
        //    try
        //    {
        //        if (_repo.Single<Mentor>(x => x.PhoneNumber == incomingSms.FromPhoneNumber) != null)
        //            return true;
        //        if (_repo.Single<Judge>(x => x.PhoneNumber == incomingSms.FromPhoneNumber) != null)
        //            return true;
        //        if (_repo.Single<Team>(x => x.PhoneNumber == incomingSms.FromPhoneNumber) != null)
        //            return true;
        //    }
        //    catch (Exception exception)
        //    {
        //        _smsService.SendSms(incomingSms.FromPhoneNumber, exception.ToString());
        //        _logger.Log(exception);
        //    }
        //    return false;
        //}
    }
}
