using System;

namespace GamePlayerBuilder.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public GameTypes Type { get; set; }
        public DateTime Started { get; set; }
        public decimal MinBet { get; set; }
        public int MaxNumberOfHands { get; set; }
    }
}
