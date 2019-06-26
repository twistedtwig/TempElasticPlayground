using System;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticGame
    {
        public Guid Id { get; set; }
        public GameTypes Type { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int StartDay { get; set; }
        public DateTime Started { get; set; }
        public decimal MinBet { get; set; }
    }
}
