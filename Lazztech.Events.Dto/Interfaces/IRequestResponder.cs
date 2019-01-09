using HackathonManager.Models;

namespace HackathonManager.Sms
{
    public interface IRequestResponder
    {
        void MentorRequestResponse(MentorRequest mentorRequest);
    }
}