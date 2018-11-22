using System;
using HackathonManager.RepositoryPattern;
using HackathonManager.MongoDB;
using HackathonManager.Interfaces;
using HackathonManager.Sms;

namespace HackathonManager.DIContext
{
    public static class Context
    {
        /// <summary>
        /// No connection string so it defaults to the localhost and standard port.
        /// </summary>
        /// <returns></returns>
        public static IRepository GetLocalMongoDbRepo()
        {
            var repository = new MongoRepository();
            return repository;
        }

        public static IRepository GetMLabsMongoDbRepo()
        {
            //var connectionString = $@"mongodb://<{Credentials.MLabsUsername}>:<{Credentials.MLabsPassword}>@ds014658.mlab.com:14658/hackathonmanager";
            var connectionString = $@"mongodb://{Credentials.MLabsUsername}:{Credentials.MLabsPassword}@ds014658.mlab.com:14658/hackathonmanager";
            var repository = new MongoRepository(connectionString);
            return repository;
        }

        //public static IRequestResponder GetRequestResponder()
        //{
        //    //return new 
        //}

        public static ISmsService GetTwilioSmsService()
        {
            var smsProvider = new TwilioSmsService();
            return smsProvider;
        }
    }
}
