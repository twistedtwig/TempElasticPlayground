using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace GamePlayerBuilder.Helpers
{
    public static class FullScrollDataHelper
    {
        public static async Task<IEnumerable<T>> RockAndScroll<T>(this ElasticClient client,
            string indexName,
            string scrollTimeoutMilliseconds = "2m",
            int scrollPageSize = 1000
        ) where T : class
        {
            var searchResponse = await client.SearchAsync<T>(sd => sd
                .Index(indexName)
                .From(0)
                .Take(scrollPageSize)
                .MatchAll()
                .Scroll(scrollTimeoutMilliseconds));

            var results = new List<T>();

            while (true)
            {
                if (!searchResponse.IsValid || string.IsNullOrEmpty(searchResponse.ScrollId))
                    throw new Exception($"Search error: {searchResponse.ServerError.Error.Reason}");

                if (!searchResponse.Documents.Any())
                    break;

                results.AddRange(searchResponse.Documents);
                searchResponse = await client.ScrollAsync<T>(scrollTimeoutMilliseconds, searchResponse.ScrollId);
            }

            await client.ClearScrollAsync(new ClearScrollRequest(searchResponse.ScrollId));

            return results;
        }
    }
}
