using HackathonManager.PocoModels;
using HackathonManager.RepositoryPattern;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.Tests
{
    [TestFixture]
    public class PinGeneratorTests
    {
        [Test]
        public void test()
        {
            //Arrange
            var repo = new PinGenRepoMock();
            var generator = new PinGenerator(repo);
            generator._proposedPin = 1234;

            //Act
            var result = generator.GenerateNewTeamPin(new Team());

            //Assert
            Assert.That(() => result != 1234);
        }

        #region Helper
        
        #endregion
    }

    internal class PinGenRepoMock : IRepository
    {
        public void Add<T>(T item) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            //var teams = new List<Team>();
            //teams.Add(new Team { PinNumber = 1234 });
            //return teams.AsQueryable<T>();
            return new List<T>().AsQueryable();
        }

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T item) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
