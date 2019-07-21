using System;
using System.Collections.Generic;
using GamePlayerBuilder.Elastic.Models;
using GamePlayerBuilder.Models;
using Mortware.DataFactory;

namespace GamePlayerBuilder
{
    public class PlayerGenerator
    {
        private readonly Random Random;

        public PlayerGenerator()
        {
            Random = new Random();
        }

        public IEnumerable<User> Setup(GameSetupInfo setup)
        {
            for (int i = 0; i < setup.NumberOfPlayers; i++)
            {
                yield return CreateUser(setup);
            }
        }

        public IEnumerable<User> Setup(int numberOfPlayers, GameSetupInfo setup)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                yield return CreateUser(setup);
            }
        }

        public  User Convert(ElasticUser eUser, GameSetupInfo setupInfo)
        {
            var user = new User
            {
                Id = eUser.UserId,
                MaxHandsPerDay = CalculateMaxHandsPerDay(setupInfo),
                MaxVisitsPerWeek = CalculateMaxVisitsPerWeek(setupInfo),
                Name = eUser.Name,
            };

            return user;
        }

        private User CreateUser(GameSetupInfo setupInfo)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                MaxHandsPerDay = CalculateMaxHandsPerDay(setupInfo),
                MaxVisitsPerWeek = CalculateMaxVisitsPerWeek(setupInfo),
                Name = NameGenerator.Name()
            };

            return user;
        }

        private int CalculateMaxHandsPerDay(GameSetupInfo setupInfo)
        {
            var num = Random.Next(1, setupInfo.AverageHandsPerUserPerDay);
            var variance = num * setupInfo.PercentageVarianceOnAverageHands;
            num = num + System.Convert.ToInt32(num * variance);

            return num;
        }

        private int CalculateMaxVisitsPerWeek(GameSetupInfo setupInfo)
        {
            var num = Random.Next(1, setupInfo.AverageVisitsPerWeek);
            var variance = num * setupInfo.PercentageVarianceOnVisitsPerWeek;
            num = num + System.Convert.ToInt32(num * variance);
            if (num > 7) return 7;

            return num;
        }
    }


}
