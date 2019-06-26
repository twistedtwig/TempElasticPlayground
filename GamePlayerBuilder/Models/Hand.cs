using System;
using System.Collections.Generic;

namespace GamePlayerBuilder.Models
{
    public class Hand
    {
        public Hand()
        {
            Rounds = new List<BettingRound>();
        }

        public Guid Id { get; set; }
        public Game Game { get; set; }
        public DateTime Started { get; set; }
        public List<User> Players { get; set; }
        public List<BettingRound> Rounds { get; set; }
    }
}
