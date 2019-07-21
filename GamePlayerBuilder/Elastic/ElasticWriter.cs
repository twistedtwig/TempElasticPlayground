using System;
using System.Collections.Generic;
using GamePlayerBuilder.Elastic.Models;
using GamePlayerBuilder.Models;
using Nest;

namespace GamePlayerBuilder.Elastic
{
    public class ElasticWriter
    {
        public void BulkWriteData<T>(List<T> data, ElasticClient client) where T : class
        {
            client.BulkAll(data, b => b
                    .Index(ElasticClientManager.GetIndexName<T>())
                    .BackOffTime("30s")
                    .BackOffRetries(2)
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(100)
                )
                .Wait(TimeSpan.FromMinutes(15), next => { Console.Write("."); });
        }

        public void WriteAllData<T>(List<T> data) where T : class
        {
            Console.WriteLine("");
            Console.WriteLine($"starting to write {data.Count} {typeof(T).Name} entries.");
            var converter = new ElasticClassConverter();
            var client = ElasticClientManager.CreateClient();

            var type = typeof(T);
            if (data is List<User> users)
            {
                BulkWriteData(converter.Convert(users), client);
            }

            if (data is List<ElasticUserStat> userStats)
            {
                BulkWriteData(userStats, client);
            }

            if (data is List<ElasticBettingStat> bettingStats)
            {
                BulkWriteData(bettingStats, client);
            }

        }
    }
}
