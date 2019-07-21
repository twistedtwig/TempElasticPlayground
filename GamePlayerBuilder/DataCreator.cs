using System;
using System.Collections.Generic;
using GamePlayerBuilder.Helpers;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder
{
    public class DataCreator
    {
        public void CreateDataAndWriteToElastic(GameSetupInfo info, List<User> users)
        {
            var startTime = DateTime.Now;

            Console.WriteLine("RANDOM STATS GAME GENERATOR");
            Console.WriteLine("===========================");

            var userTotal = 0;
            var handTotal = 0;

            userTotal = users.Count;

            var infoByYear = SplitByYear(info);

            foreach (var yearlyInfo in infoByYear)
            {
                Console.WriteLine("");
                Console.WriteLine($"Creating Data for year {yearlyInfo.StartDate.Year}...");
                Console.WriteLine("");

                var data = CreateData(yearlyInfo, users);

                handTotal += data;

                Console.WriteLine("");
                Console.WriteLine($"Year {yearlyInfo.StartDate.Year} Created and Saved to elastic.");
                Console.WriteLine("");
            }

            var timeTaken = startTime.TimedNowToFriendlyDisplay();

            Console.WriteLine("===========================");
            Console.WriteLine($"ALL DATA CREATED IN - {timeTaken}");
            Console.WriteLine("");

            Console.WriteLine($"Users: {userTotal.ToString("N0")}");
            Console.WriteLine($"Hands: {handTotal.ToString("N0")}");

            Console.WriteLine("===========================");

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private int CreateData(GameSetupInfo setupInfo, List<User> users)
        {
            var startTime = DateTime.Now;

            var generator = new StatGenerator(users);
            var handTotal = generator.Setup(setupInfo);

            var timeTaken = startTime.TimedNowToFriendlyDisplay();

            Console.WriteLine("");
            Console.WriteLine($"Generating Data took {timeTaken}");
            Console.WriteLine("");
            Console.WriteLine($"Hands: {handTotal.ToString("N0")}");
            Console.WriteLine("===========================");
            Console.WriteLine("");

            return handTotal;
        }

        private static IEnumerable<GameSetupInfo> SplitByYear(GameSetupInfo info)
        {
            var years = Convert.ToInt32((info.EndDate - info.StartDate).TotalDays / 365);

            for (int i = 0; i < years; i++)
            {
                yield return new GameSetupInfo
                {
                    StartDate = info.StartDate.AddYears(i),
                    EndDate = info.StartDate.AddYears(i + 1),
                    AverageHandsPerUserPerDay = info.AverageHandsPerUserPerDay,
                    AverageVisitsPerWeek = info.AverageVisitsPerWeek,
                    NumberOfPlayers = info.NumberOfPlayers,
                    PercentageVarianceOnAverageHands = info.PercentageVarianceOnAverageHands,
                    PercentageVarianceOnVisitsPerWeek = info.PercentageVarianceOnVisitsPerWeek
                };
            }
        }

    }
}
