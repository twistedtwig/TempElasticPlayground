using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using GamePlayerBuilder.Elastic.Models;
using GamePlayerBuilder.Models;
using Nest;

namespace GamePlayerBuilder.Elastic
{
    public class ElasticWriter
    {
        public void WriteData(DataResult data)
        {
            var client = CreateClient();

            WriteData(Convert(data.Games), client);
            WriteData(Convert(data.Users), client);
            //            WriteData(Convert(data.Hands), client);
            //            WriteData(Convert(data.Rounds), client);
            //            WriteData(Convert(data.Bets), client);
            //            WriteData(Convert(data.Winnings), client);
        }

        private ElasticClient CreateClient()
        {
            var settings = new ConnectionSettings(new Uri("http://example.com:9200"))
                .RequestTimeout(TimeSpan.FromMinutes(2))
                .DefaultMappingFor<ElasticGame>(g =>
                    g.IndexName("Games")
                    .TypeName("Game")
                    .IdProperty(x => x.Id)
                )
                .DefaultMappingFor<ElasticUser>(u =>
                    u.IndexName("Users")
                    .TypeName("User")
                    .IdProperty(x => x.Id)
                );

            var client = new ElasticClient(settings);

            return client;
        }

        private void WriteData<T>(List<T> data, ElasticClient client) where T : class
        {
            foreach (var item in data)
            {
                var indexResponse = client.IndexDocument(item);

                // this will fail (response has) with 

                /*
                 * {Unsuccessful low level call on PUT: /Games/Game/90a6abb0-4bb9-42f2-a0ab-35b5878eef6a}
                 * {"A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond"}
                 *
                 * BadRequest: Node: http://example.com:9200/ Took: 00:00:21.5565631
                 *
                 */
            }
        }

        private List<ElasticBet> Convert(List<Bet> bets)
        {
            var eBets = new List<ElasticBet>();
            foreach (var bet in bets)
            {
                eBets.Add(new ElasticBet
                {
                    Id = bet.Id,
                    PlayerId = bet.Player.Id,
                    RoundId = bet.Round.Id,
                    Value = bet.Value
                });
            }

            return eBets;
        }

        private List<ElasticGame> Convert(List<Game> games)
        {
            var eGames = new List<ElasticGame>();
            foreach (var game in games)
            {
                eGames.Add(new ElasticGame
                {
                    Id = game.Id,
                    Started = game.Started,
                    StartYear = game.Started.Year,
                    StartMonth = game.Started.Month,
                    StartDay = game.Started.Day,
                    Type = game.Type,
                    MinBet = game.MinBet
                });
            }

            return eGames;
        }

        private List<ElasticUser> Convert(List<User> users)
        {
            var eUsers = new List<ElasticUser>();
            foreach (var user in users)
            {
                eUsers.Add(new ElasticUser
                {
                    Id = user.Id,
                    Name = user.Name
                });
            }

            return eUsers;
        }

        private List<ElasticHand> Convert(List<Hand> hands)
        {
            var eHands = new List<ElasticHand>();
            foreach (var hand in hands)
            {
                eHands.Add(new ElasticHand
                {
                    Id = hand.Id,
                    Started = hand.Started,
                    StartYear = hand.Started.Year,
                    StartMonth = hand.Started.Month,
                    StartDay = hand.Started.Day,
                    GameId = hand.Game.Id,
                    PlayerIds = hand.Players.Select(p => p.Id).ToList()
                });
            }

            return eHands;
        }

        private List<ElasticBettingRound> Convert(List<BettingRound> rounds)
        {
            var eRounds = new List<ElasticBettingRound>();
            foreach (var round in rounds)
            {
                eRounds.Add(new ElasticBettingRound
                {
                    Id = round.Id,
                    HandId = round.Hand.Id,
                    BetIds = round.Bets.Select(b => b.Id).ToList()
                });
            }

            return eRounds;
        }

        private List<ElasticWinnings> Convert(List<Winnings> winnings)
        {
            var eWinnings = new List<ElasticWinnings>();
            foreach (var winning in winnings)
            {
                eWinnings.Add(new ElasticWinnings
                {
                    Id = winning.Id,
                    Value = winning.Value,
                    HandId = winning.Hand.Id,
                    PlayerId = winning.Player.Id
                });
            }

            return eWinnings;
        }
    }
}
