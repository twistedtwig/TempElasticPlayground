using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayerBuilder.Elastic;
using GamePlayerBuilder.Elastic.Models;
using GamePlayerBuilder.Helpers;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class StatGenerator
    {
        private readonly List<User> _users;

        private int _hands = 0;

        private readonly Random _random;

        private readonly GameGenerator _gameGenerator;
        private readonly HandGenerator _handGenerator;
        private readonly RandomBetGenerator _betGenerator;
        private readonly WinningsGenerator _winningsGenerator;

        public StatGenerator(List<User> users)
        {
            _users = users;
            
            _random = new Random();

            _gameGenerator = new GameGenerator();
            _handGenerator = new HandGenerator();
            _betGenerator = new RandomBetGenerator();
            _winningsGenerator = new WinningsGenerator();
        }

        public int Setup(GameSetupInfo setup)
        {
            var weekDates = SplitIntoWeeklyDates(setup);

            foreach (var weekDate in weekDates)
            {
                var userHandStats = new List<ElasticUserStat>();

                Console.WriteLine("");
                Console.WriteLine($"{setup.StartDate.Year} - {weekDate.GetIso8601WeekOfYear()} week started...");

                ResetPlayersGamesPlayedCounter();
                var maxNumberOfGamesToPlay = _random.Next(5, 500);
                for (int i = 0; i < maxNumberOfGamesToPlay; i++)
                {
                    var shouldStopEarly = _random.Next(0, 100) > 95;
                    if (shouldStopEarly) break;

                    var game = _gameGenerator.Create(weekDate);

                    ResetPlayersHandsPlayedCounter();

                    var previousPlayersInHand = new List<User>();
                    for (int j = 0; j < game.MaxNumberOfHands; j++)
                    {
                        var hand = _handGenerator.Create(game, _users, previousPlayersInHand, j);
                        if (hand == null) break;

                        _hands++;

                        previousPlayersInHand = hand.Players;

                        //play hand
                        var playersInHand = hand.Players;
                        for (int k = 0; k < 4; k++)
                        {
                            var round = _betGenerator.CreateRound(playersInHand, k, hand, game);

                            playersInHand = round.Bets.Where(b => b.Value > 0).Select(b => b.Player).ToList();

                            if (playersInHand.Count == 1) break;
                        }

                        var winningTemp = _winningsGenerator.Create(hand, playersInHand);
                        userHandStats.AddRange(ConvertToElasticData(hand, winningTemp));
                    }
                }

                var writer = new ElasticWriter();
                Console.WriteLine($"Writing week {weekDate.GetIso8601WeekOfYear()}");
                
                writer.WriteAllData(userHandStats);
            }

            return _hands;
        }

        private List<ElasticUserStat> ConvertToElasticData(Hand hand, List<Winnings> winnings)
        {
            var userStats = new List<ElasticUserStat>();

            foreach (var player in hand.Players)
            {
                var isAWinner = winnings.Any(w => w.Player.Id == player.Id);

                var userStat = new ElasticUserStat
                {
                    HandId = hand.Id,
                    UserId = player.Id,
                    GameType = (int)hand.Game.Type,
                    GroupId = null,
                    TournamentId = null,
                    When = hand.Started,
                    WhenYear = hand.Started.Year,
                    WhenMonth = hand.Started.Month,
                    WhenDay = hand.Started.Day,

                    PlayersInStart = hand.Rounds.OrderBy(x => x.Round).First().Bets.Select(b => b.Player).Distinct().Count(),
                    PlayersAtEnd = hand.Rounds.OrderByDescending(x => x.Round).First().Bets.Select(b => b.Player).Distinct().Count(),
                    
                    PostSize = hand.Rounds.Sum(r => r.Bets.Sum(b => b.Value)),
                    WinValue = isAWinner ? winnings.Where(w => w.Player.Id == player.Id).Sum(w => w.Value) : (decimal?)null,
                    Won = isAWinner
                };

                userStats.Add(userStat);
            }

            return userStats;
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
