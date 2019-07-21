using System;

namespace GamePlayerBuilder.Elastic.Models
{
    public abstract class ElasticGameBaseStat
    {
        public Guid? TournamentId { get; set; }
        public Guid? GroupId { get; set; }
        public int GameType { get; set; }
        public Guid UserId { get; set; }

        public Guid HandId { get; set; }
        public DateTime When { get; set; }
        public int WhenYear { get; set; }
        public int WhenMonth { get; set; }
        public int WhenDay { get; set; }
    }

    public class ElasticUserStat : ElasticGameBaseStat
    {
        public bool Won { get; set; }
        public decimal PostSize { get; set; }
        public decimal? WinValue { get; set; }
        public int PlayersInStart { get; set; }
        public int PlayersAtEnd { get; set; }
    }

    public class ElasticBettingStat : ElasticGameBaseStat
    {

    }
}
