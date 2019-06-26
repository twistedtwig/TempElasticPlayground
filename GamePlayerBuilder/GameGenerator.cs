using System;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class GameGenerator
    {
        private readonly Random _random;

        public GameGenerator()
        {
            _random = new Random();
        }

        public Game Create(DateTime startDate)
        {
            Array values = Enum.GetValues(typeof(GameTypes));
            GameTypes gameType = (GameTypes)values.GetValue(_random.Next(values.Length));

            var date = startDate.AddDays(_random.Next(0, 6));
            date = date.AddHours(_random.Next(0, 23));
            date = date.AddMinutes(_random.Next(0, 59));

            var game = new Game
            {
                Id = Guid.NewGuid(),
                MinBet = 5* (int)Math.Round(_random.Next(5,100) / 5d),
                Started = date,
                Type = gameType,
                MaxNumberOfHands = _random.Next(10, 500),
            };

            return game;
        }
    }
}
