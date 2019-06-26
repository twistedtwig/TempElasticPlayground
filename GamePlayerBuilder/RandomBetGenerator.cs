using System;
using System.Collections.Generic;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class RandomBetGenerator
    {
        private readonly Random _random;

        public RandomBetGenerator()
        {
            _random = new Random();
        }

        public BettingRound CreateRound(List<User> playersInRound, int roundNumber, Hand hand, Game game)
        {
            var round = new BettingRound
            {
                Id = Guid.NewGuid(),
                Hand = hand,
                Round = roundNumber,
            };

            hand.Rounds.Add(round);

            var hasOnePersonBet = false;
            foreach (var user in playersInRound)
            {
                var betValue = _random.Next(hasOnePersonBet ? 0 : 1, 4);
                if (betValue > 0)
                {
                    betValue = Convert.ToInt32(game.MinBet * _random.Next(1, 4));
                    hasOnePersonBet = true;
                }

                round.Bets.Add(new Bet
                {
                    Id = Guid.NewGuid(),
                    Round = round,
                    Player = user,
                    Value = betValue
                });
            }

            return round;
        }
    }
}
