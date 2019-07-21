using System.Collections.Generic;
using GamePlayerBuilder.Models;

namespace GamePlayerBuilder.Elastic.Models
{
    public class ElasticClassConverter
    {
        public List<ElasticUser> Convert(List<User> users)
        {
            var eUsers = new List<ElasticUser>();
            foreach (var user in users)
            {
                eUsers.Add(new ElasticUser
                {
                    UserId = user.Id,
                    Name = user.Name
                });
            }

            return eUsers;
        }
    }
}
