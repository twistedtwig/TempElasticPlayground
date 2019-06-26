using System;

namespace GamePlayerBuilder.Models
{
    public class GameSetupInfo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfPlayers { get; set; }
        public int AverageHandsPerUserPerDay { get; set; }
        public double PercentageVarianceOnAverageHands { get; set; }
        public int AverageVisitsPerWeek { get; set; }
        public double PercentageVarianceOnVisitsPerWeek { get; set; }
    }
}
