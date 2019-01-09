using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IRequestResponder
    {
        void MentorRequestResponse(MentorRequest mentorRequest);
    }
}