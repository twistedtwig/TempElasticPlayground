using System;
using System.Linq;
using System.Text;
using GamePlayerBuilder.Elastic.Models;
using GamePlayerBuilder.Helpers;
using Nest;

namespace GamePlayerBuilder.Elastic
{
    public class ElasticReader
    {
        private readonly ElasticClient _client;

        public ElasticReader()
        {
            _client = ElasticClientManager.CreateClient();
        }

        public void ListAllIndexes()
        {
            var startTime = DateTime.Now;
            var indexes = _client.Cat.Indices().Records;

            Console.WriteLine("indexes");
            Console.WriteLine("-------");
            foreach (var index in indexes)
            {
                Console.WriteLine($"{index.Index}");
            }

            WriteEndTimer("Listing all indexes", startTime);
            Console.WriteLine("-------");
        }

        public Guid GetUserId()
        {
            var startTime = DateTime.Now;
            var userId = _client.Search<ElasticUser>(s => s.Query(q => q.MatchAll()).Size(1)).Documents.First().UserId;
            WriteEndTimer("Getting first userId", startTime);
            return userId;
        }
        
//        public void UserStats(Guid userId)
//        {
//            Console.WriteLine("");
//            Console.WriteLine($"User stats: {userId}");
//            Console.WriteLine("");
//
//            var startTime = DateTime.Now;
//            var bets =_client.Search<ElasticBet>(s =>
//                s.Query(q =>
//                    q.Match(m =>
//                        m.Field(f => f.PlayerId).Query(userId.ToString())))
//                .Size(9999));
//            // obviously this would be done with an aggregation, just experimenting here.
//
//            Console.WriteLine($"User bets {bets.Total} {bets.Documents.Count} {bets.Hits.Count} - bet a total of: {bets.Documents.Sum(b => b.Value)}");
//
//            var hands = _client.Search<ElasticHand>(s =>
//                s.Query(q =>
//                    q.Match(m => 
//                        m.Field(f => f.PlayerIds).Query(userId.ToString())))
//                    .Size(0));
//
//            var handsQuery = ToJson(hands);
//
//            Console.WriteLine($"User hands {hands.Total}");
//
//            var dates2016 = _client.Search<ElasticHand>(s => 
//                s.Query(q => 
//                    q.Match(m => m.Field(f => f.StartYear).Query("2016"))));
//                            
//
//            var datesRequest = ToJson(dates2016);
//
//
//            var handsBetweenTwoDatesForUser = _client.Search<ElasticHand>(s =>
//                s.Query(q => q.Bool(b => b
//                    .Must(
//                        mst => mst.Match(m => m.Field(f => f.PlayerIds).Query(userId.ToString())),
//                        mst => mst.Match(m => m.Field(f => f.StartMonth).Query(5.ToString())),
//                        mst => mst.DateRange(r => r.Field(f => f.Started)
//                                .GreaterThanOrEquals(new DateTime(2017, 1, 1))
//                                .LessThan(new DateTime(2018, 1, 1)))
//                    )
//                )));
//
//            Console.WriteLine($"user hands between {handsBetweenTwoDatesForUser.Total} {handsBetweenTwoDatesForUser.Hits.Count} {handsBetweenTwoDatesForUser.Documents.Count}");
//            var handsDateRequest = ToJson(handsBetweenTwoDatesForUser);
//
//
//            WriteEndTimer("getting user stats", startTime);
//        }

        public void EachUserWinnings()
        {
            var startTime = DateTime.Now;

            Console.WriteLine("Getting aggregation of user winnings");
            Console.WriteLine("");

            var users = _client.RockAndScroll<ElasticUser>(StringConstants.UserIndex).Result.ToList();

            var result = _client.Search<ElasticWinnings>(s =>
                s.Aggregations(a =>
                    a.Terms("term_Agg", t =>
                        t.Field(f => f.PlayerId)
                            .Aggregations(aa =>
                                aa.Sum("sum", sum => 
                                    sum.Field(f => f.Value))
                                )
                        ))
                );

            var resultRequest = ToJson(result);

            var aggs = result.Aggregations.Terms("term_Agg");

            foreach (var bucket in aggs.Buckets)
            {
                var user = users.Single(u => u.UserId == Guid.Parse(bucket.Key));
                var sum = bucket.Values.First() as ValueAggregate;
                var winnings = sum.Value.HasValue ? sum.Value.Value.ToString("C") : "none";
                Console.WriteLine($"{user.Name} - {winnings}");
            }

            Console.WriteLine("Getting aggregation of a filtered (subset of users) winnings");
            Console.WriteLine("");

            var threeUserIds = users.Take(3).Select(u => u.UserId).ToList();
            var userWinningsAgg = _client.Search<ElasticWinnings>(s =>
                s.Query(q => q.Terms(t => t.Field(f => f.PlayerId).Terms(threeUserIds)))
                .Aggregations(a =>
                    a.Terms("term_Agg", t =>
                        t.Field(f => f.PlayerId)
                            .Aggregations(aa =>
                                aa.Sum("sum", sum =>
                                    sum.Field(f => f.Value))
                            )
                    ))
            );

            var filterUserTermsAggregate = userWinningsAgg.Aggregations.Terms("term_Agg");

            foreach (var bucket in filterUserTermsAggregate.Buckets)
            {
                var user = users.Single(u => u.UserId == Guid.Parse(bucket.Key));
                var sum = bucket.Values.First() as ValueAggregate;
                var winnings = sum.Value.HasValue ? sum.Value.Value.ToString("C") : "none";
                Console.WriteLine($"{user.Name} - {winnings}");
            }

            Console.WriteLine("");
            WriteEndTimer("average user winnings", startTime);
        }

        public void ReadComplexBetsQuery()
        {
            var startTime = DateTime.Now;

            //want to query against bets for 50 users
            //want it between two date ranges
            //want the best to be greater than 50 and less than 500

            //aggregate the results into number of bets, sum and average


            WriteEndTimer("complex bets query", startTime);

        }

        public void CountAllTypes()
        {
            var startTime = DateTime.Now;
            Count<ElasticUser>();
            Count<ElasticUserStat>();
            WriteEndTimer("counting all types", startTime);
        }

        private void Count<T>() where T : class
        {
            var result = _client.Count<T>(s => s.Query(q =>
                    q.MatchAll())
            );

            Console.WriteLine($"Counting {typeof(T).Name} {result.Count.ToString("N0")}");
        }

        private void WriteEndTimer(string message, DateTime startTime)
        {
            var timeTaken = startTime.TimedNowToFriendlyDisplay();
            Console.WriteLine($"{message} took {timeTaken}");
        }

        public static string ToJson(IResponse response)
        {
            return Encoding.UTF8.GetString(response.ApiCall.RequestBodyInBytes);
        }
    }
}
