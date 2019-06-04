using System;

namespace SSO.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    public class UserService : IUserService
    {
        private readonly List<User> _store = new List<User>();

        public UserService()
        {
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < 100; i++)
            {
                var user = new User
                {
                    Name = $"Tony Xiao - {i}",
                    Email = $"toxiao{i}@microsoft.com",
                    Department = $"Depart-{i}",
                    Alias = $"toxiao{i}"
                };
                this._store.Add(user);
            }
        }

        public User FindUser(string userName)
        {
            return this._store.Where(x => x.Alias == userName.ToLower()).FirstOrDefault();
        }

        public User FindUser(Func<User, bool> filter)
        {
            return this._store.Where(filter).FirstOrDefault();
        }
    }
}