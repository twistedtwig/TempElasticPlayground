using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class StatGenerator
    {
        private List<User>_users;
        private List<Game> _games;
        private List<Hand> _hands;
        private List<BettingRound> _rounds;
        private List<Bet> _bets;
        private List<Winnings> _winnings;

        private readonly Random _random;

        private readonly PlayerGenerator _playerGenerator;
        private readonly GameGenerator _gameGenerator;
        private readonly HandGenerator _handGenerator;
        private readonly RandomBetGenerator _betGenerator;
        private readonly WinningsGenerator _winningsGenerator;

        public StatGenerator()
        {
            _users = new List<User>();
            _games = new List<Game>();
            _hands = new List<Hand>();
            _rounds = new List<BettingRound>();
            _bets = new List<Bet>();
            _winnings = new List<Winnings>();

            _random = new Random();

            _playerGenerator = new PlayerGenerator();
            _gameGenerator = new GameGenerator();
            _handGenerator = new HandGenerator();
            _betGenerator = new RandomBetGenerator();
            _winningsGenerator = new WinningsGenerator();
        }
        public DataResult Setup(GameSetupInfo setup)
        {
            _users = _playerGenerator.Setup(setup).ToList();

            var weekDates = SplitIntoWeeklyDates(setup);

            foreach (var weekDate in weekDates)
            {
                ResetPlayersGamesPlayedCounter();
                var maxNumberOfGamesToPlay = _random.Next(5, 500);
                for (int i = 0; i < maxNumberOfGamesToPlay; i++)
                {
                    var shouldStopEarly = _random.Next(0, 100) > 95;
                    if (shouldStopEarly) break;

                    var game = _gameGenerator.Create(weekDate);
                    _games.Add(game);
                    ResetPlayersHandsPlayedCounter();

                    var previousPlayersInHand = new List<User>();
                    for (int j = 0; j < game.MaxNumberOfHands; j++)
                    {
                        var hand = _handGenerator.Create(game, _users, previousPlayersInHand, j);
                        if (hand == null) break;
                        _hands.Add(hand);
                        previousPlayersInHand = hand.Players;

                        //play hand
                        var playersInHand = hand.Players;
                        for (int k = 0; k < 4; k++)
                        {
                            var round = _betGenerator.CreateRound(playersInHand, k, hand, game);
                            _rounds.Add(round);
                            playersInHand = round.Bets.Where(b => b.Value > 0).Select(b => b.Player).ToList();

                            _bets.AddRange(round.Bets);

                            if (playersInHand.Count == 1) break;
                        }

                        _winnings.AddRange(_winningsGenerator.Create(hand, playersInHand));
                    }
                }
            }

            return new DataResult
            {
                Users = _users,
                Games = _games,
                Hands = _hands,
                Rounds = _rounds,
                Bets = _bets,
                Winnings = _winnings
            };
        }

        private List<DateTime> SplitIntoWeeklyDates(GameSetupInfo setup)
        {
            if (setup.StartDate >= setup.EndDate)
            {
                throw new ArgumentException("start date is newer than end date");
            }

            var dates = new List<DateTime>();
            dates.Add(setup.StartDate);
            var nextDate = setup.StartDate.AddDays(7);

            while (nextDate < setup.EndDate)
            {
                dates.Add(nextDate);
                nextDate = nextDate.AddDays(7);
            }

            return dates;
        }

        private void ResetPlayersHandsPlayedCounter()
        {
            foreach (var user in _users)
            {
                user.ResetHandsPlayed();
            }
        }

        private void ResetPlayersGamesPlayedCounter()
        {
            foreach (var user in _users)
            {
                user.ResetGamesPlayed();
                user.ResetHandsPlayed();
            }
        }
    }
}
