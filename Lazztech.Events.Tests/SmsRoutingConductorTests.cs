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
        public void PairedRequestResponse_Y_RequestShouldBeMarkedAccepted()
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
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
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
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault().RequestAccepted == true);
        }

        [Fact]
        public void PairedRequestResponse_YES_RequestShouldBeMarkedAccepted()
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
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            var smsResponse = new SmsDto(message: "YES", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            Assert.DoesNotContain(SmsRoutingConductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(SmsRoutingConductor.MentorRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault().RequestAccepted == true);
        }

        [Fact]
        public void PairedRequestResponse_N_RequestShouldBeMarkeNotAccepted()
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
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            var smsResponse = new SmsDto(message: "N", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            Assert.DoesNotContain(SmsRoutingConductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(SmsRoutingConductor.MentorRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault().RequestAccepted == false);
        }

        [Fact]
        public void PairedRequestResponse_NO_RequestShouldBeMarkeNotAccepted()
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
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            var smsResponse = new SmsDto(message: "NO", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            Assert.DoesNotContain(SmsRoutingConductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(SmsRoutingConductor.MentorRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault().RequestAccepted == false);
        }

        [Fact]
        public void TwoDifferentRequests_OneResponse_Y_CorrectRequestShouldBeMarkedAccepted()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new SmsRoutingConductor(repo.Object, sms.Object, responder.Object);

            var requestForGian = new MentorRequest()
            {
                TeamName = "TestTeamName123",
                Mentor = new Mentor()
                {
                    FirstName = "Gian",
                    LastName = "Lazzarini",
                    PhoneNumber = "GiansNumber123",
                },
                OutboundSms = new SmsDto(
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            var requestForMark = new MentorRequest()
            {
                TeamName = "TestTeam0",
                Mentor = new Mentor()
                {
                    FirstName = "Mark",
                    LastName = "Johnston",
                    PhoneNumber = "MarksPhoneNumber123",
                },
                OutboundSms = new SmsDto(
                    message: EventStrings.OutBoundRequestSms("Mark", "TestTeam0", "Example Room 2"),
                    toNumber: "MarksPhoneNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            var smsResponseFromGian = new SmsDto(message: "Y", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(requestForGian);
            SmsRoutingConductor.MentorRequests.Add(requestForMark);
            SmsRoutingConductor.InboundMessages.Add(smsResponseFromGian);
            conductor.ProcessMentorRequests();

            //Assert
            //Assert.DoesNotContain(SmsRoutingConductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            //Assert.DoesNotContain(SmsRoutingConductor.MentorRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault(x => x.Mentor.FirstName == "Gian").RequestAccepted == true);
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault(x => x.Mentor.FirstName == "Mark").RequestAccepted == false);
        }

        [Fact]
        public void PairedRequest_GibberishResponse_RequestShouldBeMarkeNotAcceptedButResponseShouldStillBeProccessed()
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
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            var smsResponse = new SmsDto(message: "asdf", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            SmsRoutingConductor.MentorRequests.Add(request);
            SmsRoutingConductor.InboundMessages.Add(smsResponse);
            conductor.ProcessMentorRequests();

            //Assert
            Assert.NotNull(SmsRoutingConductor.InboundMessages.FirstOrDefault().DateTimeWhenProcessed);
            Assert.Null(SmsRoutingConductor.MentorRequests.FirstOrDefault().DateTimeWhenProcessed);
            Assert.True(SmsRoutingConductor.MentorRequests.FirstOrDefault().RequestAccepted == false);
        }

    }
}
