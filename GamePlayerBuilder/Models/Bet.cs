using System;
using System.Collections.Generic;
using System.Text;

namespace GamePlayerBuilder.Models
{
    public class Bet
    {
        public Guid Id { get; set; }
        public BettingRound Round { get; set; }
        public User Player { get; set; }
        public decimal Value { get; set; }
    }
}
