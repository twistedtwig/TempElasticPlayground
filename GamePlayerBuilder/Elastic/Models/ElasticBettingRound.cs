using System;
using System.Collections.Generic;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticBettingRound
    {
        public Guid Id { get; set; }
        public Guid HandId { get; set; }
        public List<Guid> BetIds { get; set; }
    }
}
