using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace Kregle
{
    public class Game
    {
        private List<(User user,IFrame frame)> users;
        private int index = 0;

        public Game(params User[] users)
        {
            this.users = new List<(User user, IFrame frame)>();
            AddUser(users);
        }

        public void AddUser(params User[] users)
        {
            foreach (var user in users)
            {
                user.Id = this.users.Count + 1;
                this.users.Add((user, new StartFrame()));
            }
        }

        public void AddThrow(int points)
        {
            var user = users[index];
            if (user.frame.IsHalfFrame())
            {
                user.frame.AddNextThrow(points);
            }
            else
            {
                user = users[++index];
                user.frame.AddNextThrow(points);
            }
        }

        public User GetUser()
        {
            return users[index].user;
        }

        public string ToString(User user)
        {
            var builder = new StringBuilder($"{user.NickName}:\n");
            if (users.Any(u => u.user == user))
            {
                var userFrame = users.FirstOrDefault(u => u.user == user);
                foreach (var frame in userFrame.frame.GetFramsesEnumerable())
                {
                    builder.Append(frame.ToString());
                }
            }
            return builder.ToString();
        }
    }
}
