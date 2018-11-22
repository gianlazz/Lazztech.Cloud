using HackathonManager.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.Models
{
    class TeamVote
    {
        /// <summary>
        /// Volunteer that coordinates and facilitates the judges
        /// being introducesed to the teams
        /// </summary>
        public string JudgeModerator { get; set; }
        public Team Team { get; set; }
        public Judge Judge { get; set; }
        public Score Score { get; set; }
        public string PreJudgingNotes { get; set; }
    }

    public enum Score
    {
        frown,
        neutral,
        smile
    }
}
