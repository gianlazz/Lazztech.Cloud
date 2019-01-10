using Lazztech.Events.Domain.Sms;
using Lazztech.Events.Dto;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Moq;
using System;
using Xunit;

namespace Lazztech.Events.Tests
{
    public class SmsRoutingConductorTests
    {
        [Fact]
        public void MentorRequestAndAcceptance()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new SmsRoutingConductor(repo.Object, sms.Object, responder.Object);
            var request = MentorRequestHelper("exampleTeam");
            var smsResponse = new SmsDto(message: "Y", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            //Assert.True();
        }

        public MentorRequest MentorRequestHelper(string team)
        {
            var request = new MentorRequest();
            request.TeamName = "TestTeamName123";
            request.Mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            var message = EventStrings.OutBoundRequestSms("Gian", team, "Example Room");
            request.OutboundSms = new SmsDto(message: message, toNumber: "GiansNumber123", fromNumber: "TwilioNumber123");

            return request;
        }
    }
}
