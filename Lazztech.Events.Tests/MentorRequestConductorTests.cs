using Lazztech.Events.Domain;
using Lazztech.Events.Dto;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.MongoDB;
using Moq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Lazztech.Events.Tests
{
    public class SmsRoutingConductorTests
    {
        [Theory]
        [InlineData("Y")]
        [InlineData("y")]
        [InlineData("YES")]
        [InlineData("Yes")]
        [InlineData("yes")]
        public void TryAddRequestANDProcessResponse_PositiveResponse_ShouldBeMarkedAccepted(string response)
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
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

            var smsResponse = new SmsDto(message: response, toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            //Act
            var succeded = conductor.TryAddRequest(request);
            var result = conductor.ProcessResponse(smsResponse);

            //Assert
            Assert.NotNull(result.DateTimeWhenProcessed);
            Assert.True(result.RequestAccepted);
        }

        [Theory]
        [InlineData("N")]
        [InlineData("n")]
        [InlineData("NO")]
        [InlineData("No")]
        [InlineData("no")]
        public void TryAddRequestANDProcessResponse_NegativeResponse_ShouldBeMarkedNotAccepted(string response)
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
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
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: response, toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var succeded = conductor.TryAddRequest(request);
            var result = conductor.ProcessResponse(smsResponse);

            //Assert
            Assert.NotNull(result.DateTimeWhenProcessed);
            Assert.False(result.RequestAccepted);
        }

        [Fact]
        public void TryAddRequestANDProcessResponse_TwoDifferentRequestsWithOneAcceptance_CorrectRequestShouldBeMarkedAccepted()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
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
            //THIS ISN'T WORKING AND IS RETURNING NULL!
            repo.SetupSequence(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>()))
                .Returns(requestForGian.Mentor).Returns(requestForMark.Mentor)
                .Returns(requestForGian.Mentor).Returns(requestForMark.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponseFromGian = new SmsDto(message: "Y", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var succededForGian = conductor.TryAddRequest(requestForGian);
            var succededForMark = conductor.TryAddRequest(requestForMark);
            var result = conductor.ProcessResponse(smsResponseFromGian);

            //Assert
            Assert.True(result.Mentor.FirstName == "Gian" && result.RequestAccepted);
            Assert.True(conductor.Requests.Values.FirstOrDefault(x => x.Mentor.FirstName == "Mark").RequestAccepted == false);
        }

        [Fact]
        public void TryAddRequestANDProcessResponse_NonsenseResponseMessage_ShouldNotProcessAnyRequests()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
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
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);

            var smsResponse = new SmsDto(message: "asdf", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var succeded = conductor.TryAddRequest(request);
            conductor.ProcessResponse(smsResponse);

            //Assert
            Assert.Null(conductor.Requests.Values.FirstOrDefault().DateTimeWhenProcessed);
            Assert.True(conductor.Requests.Values.FirstOrDefault().RequestAccepted == false);
        }

        [Fact]
        public void TryAddRequest_AddTwoRequestsForOneMentor_ShouldNotSucceed()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
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
            repo.SetupSequence(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>()))
                .Returns(request1.Mentor).Returns(request2.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);

            //Act
            var succeded = conductor.TryAddRequest(request1);
            succeded = conductor.TryAddRequest(request2);

            //Assert
            Assert.False(succeded);
        }

        [Fact]
        public void TryAddRequest_RequestWithoutResponse_ShouldBeMarkedAvailableAfterTimeout()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
                Mentor = mentor,
                OutboundSms = new SmsDto(
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
                RequestTimeout = new System.TimeSpan(hours: 0, minutes: 0, seconds: 0),
                MentoringDuration = new System.TimeSpan(hours: 0, minutes: 0, seconds: 0)
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);

            //Act
            var succeded = conductor.TryAddRequest(request);

            //Assert
            repo.Verify(x => x.Add(It.Is<MentorRequest>(m => m.TimedOut == true)));
            Assert.Empty(conductor.Requests);
        }

        [Category("Unrealiable Test")]
        [Fact]
        public void TryAddRequestANDProcessResponse_AcceptanceResponse_ShouldNotTimeoutAfterAcceptance()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
                Mentor = mentor,
                OutboundSms = new SmsDto(
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
                RequestTimeout = new System.TimeSpan(hours: 0, minutes: 0, seconds: 1),
                MentoringDuration = new System.TimeSpan(hours: 0, minutes: 0, seconds: 0)
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: "Y", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var succeded = conductor.TryAddRequest(request);
            var result = conductor.ProcessResponse(smsResponse);

            //Assert
            repo.Verify(x => x.Add(It.Is<MentorRequest>(m => m.TimedOut == false)));
        }

        [Theory]
        [InlineData("Busy")]
        [InlineData("busy")]
        [InlineData("BUSY")]
        public void ProcessResponse_BusyResponse_MentorShouldBeMarkedAsNotAvailable(string response)
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: response, toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var result = conductor.ProcessResponse(smsResponse);
            mentor.IsAvailable = false;

            //Assert
            repo.Verify(x => x.Add<Mentor>(mentor));
            sms.Verify(x => x.SendSms(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData("Available")]
        [InlineData("available")]
        [InlineData("AVAILABLE")]
        public void ProcessResponse_AvailableResponse_MentorShouldBeMarkedAsNotAvailable(string response)
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: response, toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var result = conductor.ProcessResponse(smsResponse);

            //Assert
            mentor.IsAvailable = true;
            repo.Verify(x => x.Add<Mentor>(mentor));
            sms.Verify(x => x.SendSms(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData("Guide")]
        [InlineData("guide")]
        [InlineData("GUIDE")]
        public void ProcessResponse_GuideResponse_MentorShouldBeMarkedAsNotAvailable(string response)
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: response, toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var result = conductor.ProcessResponse(smsResponse);

            //Assert
            sms.Verify(x => x.SendSms(It.IsAny<string>(), It.Is<string>(y => y.ToLower().Contains("guide"))), Times.Once());
        }

        [Fact]
        public void ProcessResponse_NoMatchingRequest_ShouldBeIgnored()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: "asdf", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            var result = conductor.ProcessResponse(smsResponse);

            //Assert
            repo.Verify(x => x.Add<SmsDto>(It.IsAny<SmsDto>()));
            sms.Verify(x => x.SendSms(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void TryAddRequest_MentorNotAvailableOrPresent_RequestShouldNotGoThrough()
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var request = new MentorRequest()
            {
                UniqueRequesteeId = "TestTeamName123",
                Mentor = new Mentor()
                {
                    FirstName = "Gian",
                    LastName = "Lazzarini",
                    PhoneNumber = "GiansNumber123",
                    IsAvailable = false,
                    IsPresent = false
                },
                OutboundSms = new SmsDto(
                    message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
                    toNumber: "GiansNumber123",
                    fromNumber: "TwilioNumber123"),
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);

            //Act
            var succeded = conductor.TryAddRequest(request);

            //Assert
            Assert.False(succeded);
        }

        [Category("Unreliable Test")]
        [Fact]
        public void TryAddRequestANDProcessResponse_PositiveResponseThenAvailableResponse_ShouldRemoveMentorsRequest()
        {
            ////Arrange
            //var repo = new Mock<IRepository>();
            //var dal = new Mock<MongoDalHelper>(repo);
            //var sms = new Mock<ISmsService>();
            //var responder = new Mock<IRequestNotifier>();
            //var request = new MentorRequest()
            //{
            //    UniqueRequesteeId = "TestTeamName123",
            //    Mentor = new Mentor()
            //    {
            //        FirstName = "Gian",
            //        LastName = "Lazzarini",
            //        PhoneNumber = "GiansNumber123",
            //    },
            //    OutboundSms = new SmsDto(
            //        message: EventStrings.OutBoundRequestSms("Gian", "exampleTeam", "Example Room"),
            //        toNumber: "GiansNumber123",
            //        fromNumber: "TwilioNumber123"),
            //    MentoringDuration = new TimeSpan(hours: 0, minutes: 0, seconds: 1)
            //};
            //repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(request.Mentor);
            //var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            //var smsResponse = new SmsDto(message: "y", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");
            //var smsAvailableResponse = new SmsDto(message: "Available", toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            ////Act
            //var succeeded = conductor.TryAddRequest(request);
            //var result = conductor.ProcessResponse(smsResponse);
            //var result2 = conductor.ProcessResponse(smsAvailableResponse);

            ////Assert
            //Assert.Equal(expected: 0, actual: conductor.Requests.Count);
            //sms.Verify(x => x.SendSms(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Theory]
        [InlineData("y")]
        [InlineData("n")]
        public void ProcessResponse_RequestResponseWithNoRequest_HandleResponseWithNoRequest(string response)
        {
            //Arrange
            var repo = new Mock<IRepository>();
            var dal = new Mock<MongoDalHelper>(repo);
            var sms = new Mock<ISmsService>();
            var responder = new Mock<IRequestNotifier>();
            var mentor = new Mentor()
            {
                FirstName = "Gian",
                LastName = "Lazzarini",
                PhoneNumber = "GiansNumber123",
            };
            repo.Setup(x => x.Single<Mentor>(It.IsAny<Expression<Func<Mentor, bool>>>())).Returns(mentor);
            var conductor = new MentorRequestConductor(dal.Object, sms.Object, responder.Object);
            var smsResponse = new SmsDto(message: response, toNumber: "TwilioNumber123", fromNumber: "GiansNumber123");

            //Act
            conductor.ProcessResponse(smsResponse);

            //Assert
            sms.Verify(x => x.SendSms(It.IsAny<string>(), It.Is<string>(message => message.ToLower().Contains("uncertain"))));
            sms.Verify(x => x.SendSms(It.IsAny<string>(), It.Is<string>(message => message.ToLower().Contains("guide"))));
        }
    }
}