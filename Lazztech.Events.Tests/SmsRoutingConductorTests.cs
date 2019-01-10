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
            var smsResponse = SmsHelper(message: "Y");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            //Assert.True();
        }

        public SmsDto SmsHelper(string message)
        {
            var smsDto = new SmsDto();
            smsDto.Sid = "sid123";
            smsDto.DateCreated = DateTime.Now;
            smsDto.ToPhoneNumber = "TwilioNumber123";
            smsDto.FromPhoneNumber = "GiansNumber123";
            smsDto.MessageBody = message;

            return smsDto;
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
            request.OutboundSms = SmsHelper(message);

            return request;
        }
    }
}
