using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class WinningsGenerator
    {
        private readonly Random _random;

        public WinningsGenerator()
        {
            _random = new Random();
        }

        public List<Winnings> Create(Hand hand, List<User> players)
        {
            var winnings = new List<Winnings>();
            var amount = hand.Rounds.SelectMany(r => r.Bets).Sum(b => b.Value);

            var isSingleWinner = _random.Next(0, 100) > 95;
            if (isSingleWinner)
            {
                var winner = players[_random.Next(0, players.Count - 1)];
                return new List<Winnings>
                {
                    new Winnings
                    {
                         Id = Guid.NewGuid(),
                         Hand = hand,
                         Player = winner,
                         Value = amount
                    }
                };
            }

            var numberOfWinners = players.Count == 1 ? 1 : _random.Next(1, players.Count - 1);
            var eachWinningValue = amount / numberOfWinners;

            for (int i = 0; i < numberOfWinners; i++)
            {
                winnings.Add(new Winnings
                {
                    Id = Guid.NewGuid(),
                    Hand = hand,
                    Player = players[i],
                    Value = eachWinningValue
                });
            }

            return winnings;
        }
    }
}
