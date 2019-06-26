using System;

namespace GamePlayerBuilder.Models
{
    public class Winnings
    {
        public Guid Id { get; set; }
        public Hand Hand { get; set; }
        public User Player { get; set; }
        public decimal Value { get; set; }
    }
}
