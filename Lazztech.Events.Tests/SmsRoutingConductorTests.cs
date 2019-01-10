using Lazztech.Events.Domain.Sms;
using Lazztech.Events.Dto.Interfaces;
using Moq;
using System;
using Xunit;

namespace Lazztech.Events.Tests
{
    public class SmsRoutingConductorTests
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new SmsRoutingConductor(repo.Object, sms.Object, responder.Object);

            //Act

            //Assert
        }
    }
}
