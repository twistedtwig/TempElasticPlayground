using System;
using GamePlayerBuilder;
using GamePlayerBuilder.Elastic;
using GamePlayerBuilder.Models;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            Console.WriteLine("RANDOM STATS GAME GENERATOR");
            Console.WriteLine("===========================");

            var startDate = new DateTime(2018, 1,1);
            var endDate = new DateTime(2018, 2,1);
            var numOfPlayers = 10;
            var averageHandsPerUserPerDay = 200;
            var percentageVarianceOnAverageHands = 0.5;
            var averageVisitsPerWeek = 5;
            var percentageVarianceOnVisitsPerWeek = 0.5;

            var info = new GameSetupInfo
            {
                StartDate = startDate,
                EndDate = endDate,
                NumberOfPlayers = numOfPlayers,
                AverageHandsPerUserPerDay = averageHandsPerUserPerDay,
                PercentageVarianceOnAverageHands = percentageVarianceOnAverageHands,
                AverageVisitsPerWeek = averageVisitsPerWeek,
                PercentageVarianceOnVisitsPerWeek = percentageVarianceOnVisitsPerWeek
            };

            Console.WriteLine("");
            Console.WriteLine("STARTING Stat generator....");
            Console.WriteLine("");

            var generator = new StatGenerator();
            var data = generator.Setup(info);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("===========================");
            Console.WriteLine("Stat generator finished");
            Console.WriteLine("===========================");
            Console.WriteLine("");

            var endTime = DateTime.Now;
            var timeTaken = (endTime - startTime).ToFriendlyDisplay(3);

            Console.WriteLine($"Generating Data took {timeTaken}");
            Console.WriteLine("");
            Console.WriteLine("Data totals:");
            Console.WriteLine("");
            Console.WriteLine($"Users: {data.Users.Count.ToString("N0")}");
            Console.WriteLine($"Games: {data.Games.Count.ToString("N0")}");
            Console.WriteLine($"Hands: : {data.Hands.Count.ToString("N0")}");
            Console.WriteLine($"Rounds: : {data.Rounds.Count.ToString("N0")}");
            Console.WriteLine($"Bets: : {data.Bets.Count.ToString("N0")}");
            Console.WriteLine($"Winnings: : {data.Winnings.Count.ToString("N0")}");
            Console.WriteLine("===========================");
            Console.WriteLine("");
            Console.WriteLine("Beginning to write data to Elastic");
            Console.WriteLine("");

            startTime = DateTime.Now;
            var elasticWriter = new ElasticWriter();

            elasticWriter.WriteData(data);

            endTime = DateTime.Now;
            timeTaken = (endTime - startTime).ToFriendlyDisplay(3);

            Console.WriteLine($"writing Data took {timeTaken}");
            Console.WriteLine("");

            Console.ReadKey();
        }
    }
}
