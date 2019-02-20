using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IRequestNotifier
    {
        void UpdateMentorRequestee(MentorRequest mentorRequest);
        void NofityThatMentorIsntAvailable(string uniqueRequesteeId, string mentorName);
        void NofityThatMentorAvailableAgain(string mentorName);
    }
}