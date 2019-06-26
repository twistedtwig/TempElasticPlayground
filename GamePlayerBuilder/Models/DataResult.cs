using System.Collections.Generic;

namespace GamePlayerBuilder.Models
{
    public class DataResult
    {
        public List<User> Users { get; set; }
        public List<Game> Games { get; set; }
        public List<Hand> Hands { get; set; }
        public List<BettingRound> Rounds { get; set; }
        public List<Bet> Bets { get; set; }
        public List<Winnings> Winnings { get; set; }
    }
}
