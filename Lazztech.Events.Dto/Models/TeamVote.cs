using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Enums
{
    internal class TeamVote
    {
        /// <summary>
        /// Volunteer that coordinates and facilitates the judges
        /// being introducesed to the teams
        /// </summary>
        public string JudgeModerator { get; set; }

        public Team Team { get; set; }
        public Judge Judge { get; set; }
        public ScoreEnum Score { get; set; }
        public string PreJudgingNotes { get; set; }
    }
}