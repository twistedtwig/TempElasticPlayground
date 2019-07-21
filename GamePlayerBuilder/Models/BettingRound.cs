using System;
using System.Collections.Generic;

namespace GamePlayerBuilder.Models
{
    public class BettingRound
    {
        public BettingRound()
        {
            Bets = new List<Bet>();
        }
        public Guid Id { get; set; }
        public Hand Hand { get; set; }
        public int Round { get; set; }
        public List<Bet> Bets { get; set; }
    }
}
