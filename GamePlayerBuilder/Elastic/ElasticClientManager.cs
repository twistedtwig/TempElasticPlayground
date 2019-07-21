using System;
using GamePlayerBuilder.Elastic.Models;
using Nest;

namespace GamePlayerBuilder.Elastic
{
    public class ElasticClientManager
    {
        public static string GetIndexName<T>()
        {
            var t = typeof(T);

            if (t == typeof(ElasticUser)) return StringConstants.UserIndex;
            if (t == typeof(ElasticBettingStat)) return StringConstants.ElasticBettingStatIndex;
            if (t == typeof(ElasticUserStat)) return StringConstants.ElasticUserStatIndex;
            if (t == typeof(ElasticWinnings)) return StringConstants.WinningIndex;

            throw new ApplicationException();
        }

        public static ElasticClient CreateClient(bool disableDirectStreaming = true, bool isProduction = false)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DisableDirectStreaming(disableDirectStreaming)
                .RequestTimeout(TimeSpan.FromMinutes(2))
                
                .DefaultMappingFor<ElasticUser>(u => u.IndexName(StringConstants.UserIndex))
                .DefaultMappingFor<ElasticBettingStat>(g => g.IndexName(StringConstants.ElasticBettingStatIndex))
                .DefaultMappingFor<ElasticUserStat>(u => u.IndexName(StringConstants.ElasticUserStatIndex))
                .DefaultMappingFor<ElasticWinnings>(u => u.IndexName(StringConstants.WinningIndex));

            var client = new ElasticClient(settings);

            client.Indices.Create(StringConstants.UserIndex, c =>
                c.Map<ElasticUser>(m => m.AutoMap())
            );

            client.Indices.Create(StringConstants.ElasticBettingStatIndex, c =>
                c.Map<ElasticBettingStat>(m => m.AutoMap())
            );

            client.Indices.Create(StringConstants.ElasticUserStatIndex, c =>
                c.Map<ElasticUserStat>(m => m.AutoMap())
            );

            client.Indices.Create(StringConstants.WinningIndex, c =>
                c.Map<ElasticWinnings>(m => m.AutoMap())
            );

            return client;
        }
    }
}
