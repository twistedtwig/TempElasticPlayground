using System;
using System.Collections.Generic;

namespace GamePlayerBuilder.Models
{
    public class User
    {
        private readonly Random _random;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxHandsPerDay { get; set; }
        public int MaxVisitsPerWeek { get; set; }

        public HashSet<Guid> GameIds { get; set; }
        public HashSet<Guid> HandIds { get; set; }

        public User()
        {
            GameIds = new HashSet<Guid>();
            HandIds = new HashSet<Guid>();
            _random = new Random();
        }

        public bool IsAvailable()
        {
            if (GameIds.Count >= MaxVisitsPerWeek) return false;
            if (HandIds.Count >= MaxHandsPerDay) return false;
            if (_random.Next(0, 100) > 95) return false;

            return true;
        }
        
        public void AddedToHand(Hand hand)
        {
            HandIds.Add(hand.Id);
        }

        public void AddToGame(Game game)
        {
            GameIds.Add(game.Id);
        }

        public void ResetHandsPlayed()
        {
            HandIds = new HashSet<Guid>();
        }

        public void ResetGamesPlayed()
        {
            GameIds = new HashSet<Guid>();
        }
    }
}
