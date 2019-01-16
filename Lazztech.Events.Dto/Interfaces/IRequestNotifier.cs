using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IRequestNotifier
    {
        void UpdateMentorRequestee(MentorRequest mentorRequest);
    }
}