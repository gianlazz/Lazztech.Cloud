using HackathonManager.PocoModels;
using HackathonManager.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager
{
    public class PinGenerator
    {
        private readonly IRepository _Db;
        private readonly Random _random = new Random();
        /// <summary>
        /// Exposed for testing purposes.
        /// </summary>
        public int _proposedPin;

        public PinGenerator(IRepository db)
        {
            _Db = db;
        }

        //SHOULD GET TEST COVERAGE FOR THIS!
        public int GenerateNewTeamPin(Team team)
        {
            do
            {
                _proposedPin = GenerateRandomNo();
            } while (_Db.All<Team>().Where(x => x.PinNumber == _proposedPin).Any());

            return _proposedPin;
        }

        //private int GenerateRandomNo(int length)
        //{
        //    string maxString = string.Empty;
        //    for (int i = 0; i < length; i++)
        //    {
        //        maxString += "9";
        //    }

        //    int max = int.Parse(maxString);
        //    return int.Parse(_random.Next(0, max).ToString("D4"));
        //}

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

    }
}
