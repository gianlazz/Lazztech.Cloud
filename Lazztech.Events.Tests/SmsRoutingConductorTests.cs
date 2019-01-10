using Lazztech.Events.Domain.Sms;
using Lazztech.Events.Dto;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Moq;
using System;
using Xunit;
using System.Linq;

namespace Lazztech.Events.Tests
{
    public class SmsRoutingConductorTests
    {
        [Fact]
        public void ProcessRequestAcceptanceWithNoNullDateTimeWhenProcessed()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new SmsRoutingConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                TeamName = "TestTeamName123",
                Mentor = new Mentor()
                {
                    FirstName = "Gian",
                    LastName = "Lazzarini",
                    PhoneNumber = "GiansNumber123",
                },
                OutboundSms = new SmsDto(
                    EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123", 
                    fromNumber: "TwilioNumber123"),
            };

            var smsResponse = new SmsDto(message: "Y", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            Assert.DoesNotContain(SmsRoutingConductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(SmsRoutingConductor.MentorRequests, x => x.DateTimeWhenProcessed == null);
        }
    }
}
