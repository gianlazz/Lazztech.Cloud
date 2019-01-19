using HackathonManager.MongoDB;
using HackathonManager.Sms;
using Lazztech.Events.Dto.Interfaces;

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

        public static IRepository GetMLabsMongoDbRepo(string connectionString)
        {
            var repository = new MongoRepository(connectionString);
            return repository;
        }
    }
}