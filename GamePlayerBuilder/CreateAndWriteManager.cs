using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayerBuilder.Elastic;
using GamePlayerBuilder.Elastic.Models;
using GamePlayerBuilder.Helpers;
using GamePlayerBuilder.Models;
using Nest;

namespace GamePlayerBuilder
{
    public class CreateAndWriteManager
    {
        private readonly ElasticWriter _writer;
        private readonly ElasticClient _client;
        private readonly ElasticClassConverter _converter;

        private List<User> _users;

        public CreateAndWriteManager()
        {
            _client = ElasticClientManager.CreateClient();
            _writer = new ElasticWriter();
            _converter = new ElasticClassConverter();
        }

        public void Go(GameSetupInfo setupInfo)
        {
            EnsureUsersAreSetup(setupInfo);

            new DataCreator().CreateDataAndWriteToElastic(setupInfo, _users);
        }

        private void EnsureUsersAreSetup(GameSetupInfo setupInfo)
        {
            Console.WriteLine("getting existing users from elastic");
            var elasticUsersCount = _client.Count<ElasticUser>(s => s.Query(q => q.MatchAll())).Count;
            Console.WriteLine($"{elasticUsersCount} users found");

            if (elasticUsersCount < setupInfo.NumberOfPlayers)
            {
                var numberLeftToCreate = Convert.ToInt32(setupInfo.NumberOfPlayers - elasticUsersCount);
                Console.WriteLine($"need to create {numberLeftToCreate} more users");

                var users = _converter.Convert(new PlayerGenerator().Setup(numberLeftToCreate, setupInfo).ToList());

                _writer.BulkWriteData(users, _client);
            }

            var playerGenerator = new PlayerGenerator();
            _users = new List<User>();

            long userCount = 0;
            while (userCount < setupInfo.NumberOfPlayers)
            {
                Console.WriteLine("_");
                System.Threading.Thread.Sleep(10000);
                var elasticUsers = _client.RockAndScroll<ElasticUser>(StringConstants.UserIndex).Result.ToList();
                userCount = elasticUsers.Count;
                if (userCount < setupInfo.NumberOfPlayers) continue;

                //convert them back so the system can use them.
                foreach (var elasticUser in elasticUsers)
                {
                    _users.Add(playerGenerator.Convert(elasticUser, setupInfo));
                }

            }


            Console.WriteLine($"{_users.Count} users loaded");
            if(_users.Count == 0) System.Diagnostics.Debugger.Break();
        }
    }
}
