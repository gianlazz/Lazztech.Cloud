using Lazztech.Events.Domain;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Domain
{
    public class UniqueTeamFactory
    {
        private int _pinNumber;
        private readonly IRepository _Db;

        public UniqueTeamFactory(IRepository repository)
        {
            _Db = repository;
            //_pinNumber = new PinGenerator(repository).GenerateNewPin(new Team());
        }

        public Team InstantiateUniquely()
        {
            _pinNumber = new PinGenerator(_Db).GenerateNewTeamPin(new Team());
            return new Team() { PinNumber = _pinNumber };
        }
    }
}