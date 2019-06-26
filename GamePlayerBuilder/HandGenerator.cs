using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class HandGenerator
    {
        private readonly Random _random;

        public HandGenerator()
        {
            _random = new Random();
        }

        public Hand Create(Game game, List<User> users, List<User> previouslyPlayers, int handCount)
        {
            var hand = new Hand
            {
                Id = Guid.NewGuid(),
                Game = game,
                Started = game.Started.AddMinutes(5 * handCount),
            };

            /*
             * choose 2-8 players for game
	           
                picks players at random
	            keeps track of each player it has asked, so doesn't ask them twice
	            this looks at players availability and a small random number
	            if they have already played in that game it increases the odds they will continue to play further hands
	            if they are available adds to hand

	            if no one available close hand and game
             */

            var players = new List<User>();
            var numOfPlayers = _random.Next(2, 8);
            var numberOfUnavailableUsers = 0;
            var usersCount = users.Count;
            if (usersCount < numOfPlayers) return null;

            while (players.Count < numOfPlayers && usersCount - numberOfUnavailableUsers > 0)
            {
                if (previouslyPlayers != null && previouslyPlayers.Any())
                {
                    foreach (var player in previouslyPlayers)
                    {
                        if (player.IsAvailable())
                        {
                            AddPlayerToList(game, players, hand, player);
                        }
                    }
                }
                var user = users[_random.Next(0, usersCount - 1)];
                if (user.IsAvailable())
                {
                    AddPlayerToList(game, players, hand, user);
                }
                else
                {
                    numberOfUnavailableUsers++;
                }
            }

            if (!players.Any())
            {
                return null;
            }

            hand.Players = players;
            return hand;
        }

        private void AddPlayerToList(Game game, List<User> players, Hand hand, User user)
        {
            if (user == null) return;
            if (players.Any(p => p.Id == user.Id)) return;
            players.Add(user);
            user.AddedToHand(hand);
            user.AddToGame(game);
        }
    }
}
