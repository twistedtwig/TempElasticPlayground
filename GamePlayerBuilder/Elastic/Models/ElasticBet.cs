using System;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticBet
    {
        public Guid Id { get; set; }
        public Guid RoundId { get; set; }
        public Guid PlayerId { get; set; }
        public decimal Value { get; set; }
    }
}
