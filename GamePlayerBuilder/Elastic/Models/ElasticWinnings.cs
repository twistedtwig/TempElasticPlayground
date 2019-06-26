using System;
using System.Collections.Generic;
using System.Text;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticWinnings
    {
        public Guid Id { get; set; }
        public Guid HandId { get; set; }
        public Guid PlayerId { get; set; }
        public decimal Value { get; set; }
    }
}
