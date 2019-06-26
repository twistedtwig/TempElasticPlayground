using System;
using System.Collections.Generic;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticHand
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public DateTime Started { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int StartDay { get; set; }
        public List<Guid> PlayerIds { get; set; }
    }
}
