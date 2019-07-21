using System;
using Nest;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticWinnings
    {
        public Guid Id { get; set; }
        public Guid HandId { get; set; }

        [Keyword]
        public Guid PlayerId { get; set; }
        public decimal Value { get; set; }

        public string[] Cards { get; set; }
    }
}
