using Lazztech.Events.Domain;
using Lazztech.Events.Dto;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Moq;
using System.Linq;
using Xunit;

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
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessInboundSms(smsResponse);

            //Assert
            Assert.DoesNotContain(conductor.ProcessedRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(conductor.ProcessedRequests.FirstOrDefault().RequestAccepted == true);
        }

        [Fact]
        public void PairedRequestResponse_YES_RequestShouldBeMarkedAccepted()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessInboundSms(smsResponse);

            //Assert
            Assert.DoesNotContain(conductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(conductor.ProcessedRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(conductor.ProcessedRequests.FirstOrDefault().RequestAccepted == true);
        }

        [Fact]
        public void PairedRequestResponse_N_RequestShouldBeMarkeNotAccepted()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessInboundSms(smsResponse);

            //Assert
            Assert.DoesNotContain(conductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(conductor.ProcessedRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(conductor.ProcessedRequests.FirstOrDefault().RequestAccepted == false);
        }

        [Fact]
        public void PairedRequestResponse_NO_RequestShouldBeMarkeNotAccepted()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessInboundSms(smsResponse);

            //Assert
            Assert.DoesNotContain(conductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(conductor.ProcessedRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(conductor.ProcessedRequests.FirstOrDefault().RequestAccepted == false);
        }

        [Fact]
        public void TwoDifferentRequests_OneResponse_Y_CorrectRequestShouldBeMarkedAccepted()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var requestForGian = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
                UniqueRequesteeId = "TestTeam0",
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

            ////Act
            var succededForGian = conductor.TryAddRequest(requestForGian);
            var succededForMark = conductor.TryAddRequest(requestForMark);
            conductor.ProcessInboundSms(smsResponseFromGian);

            //Assert
            Assert.True(conductor.ProcessedRequests.FirstOrDefault(x => x.Mentor.FirstName == "Gian").RequestAccepted == true);
            Assert.True(conductor.UnprocessedRequests.Values.FirstOrDefault(x => x.Mentor.FirstName == "Mark").RequestAccepted == false);
        }

        [Fact]
        public void PairedRequest_GibberishResponse_RequestShouldBeMarkeNotAcceptedButResponseShouldStillBeProccessed()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessInboundSms(smsResponse);

            //Assert
            Assert.Null(conductor.UnprocessedRequests.Values.FirstOrDefault().DateTimeWhenProcessed);
            Assert.True(conductor.UnprocessedRequests.Values.FirstOrDefault().RequestAccepted == false);
        }

        [Fact]
        public void ShouldNotBeAbleToAddTwoRequestsForOneMentor()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request1 = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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

            var request2 = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeam0",
                Mentor = new Mentor()
                {
                    FirstName = "Gian",
                    LastName = "Lazzarini",
                    PhoneNumber = "GiansNumber123",
                },
                OutboundSms = new SmsDto(
                    message: EventStrings.OutBoundRequestSms("Gian", "TestTeam0", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };

            //Act
            var succeded = conductor.TryAddRequest(request1);
            succeded = conductor.TryAddRequest(request2);

            //Assert
            Assert.False(succeded);
        }

        [Fact]
        public void MentorRequestShouldMarkMentorAsAvailableAfterSetTimeout()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestResponder>();
            var conductor = new MentorRequestConductor(repo.Object, sms.Object, responder.Object);

            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
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
                RequestTimeout = new System.TimeSpan(hours: 0, minutes: 0, seconds: 1),
                MentoringDuration = new System.TimeSpan(hours: 0, minutes: 0, seconds: 0)
            };

            var smsResponse = new SmsDto(message: "YES", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessInboundSms(smsResponse);

            //Assert
            Assert.DoesNotContain(conductor.InboundMessages, x => x.DateTimeWhenProcessed == null);
            Assert.DoesNotContain(conductor.ProcessedRequests, x => x.DateTimeWhenProcessed == null);
            Assert.True(conductor.ProcessedRequests.FirstOrDefault().RequestAccepted == true);
        }
    }
}