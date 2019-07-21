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
            CreateGameData();

            ReadDataFromElastic();

            Console.ReadKey();
        }

        private static void CreateGameData()
        {
            var startDate = new DateTime(2008, 1, 1);
            var endDate = new DateTime(2009, 2, 1);
            var numOfPlayers = 10000;
            var averageHandsPerUserPerDay = 500;
            var percentageVarianceOnAverageHands = 0.3;
            var averageVisitsPerWeek = 5;
            var percentageVarianceOnVisitsPerWeek = 0.3;

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

            new CreateAndWriteManager().Go(info);
        }
        
        private static void ReadDataFromElastic()
        {
            Console.WriteLine("");
            Console.WriteLine("=======================================");
            Console.WriteLine("");
            Console.WriteLine("starting to read data from Elastic");
            Console.WriteLine("");

            var reader = new ElasticReader();

            reader.ListAllIndexes();
            reader.CountAllTypes();

            var firstUser = reader.GetUserId();
//            reader.UserStats(firstUser);

            reader.EachUserWinnings();
            reader.ReadComplexBetsQuery();
        }
    }
}
