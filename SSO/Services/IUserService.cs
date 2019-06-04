namespace SSO.Services
{
    using System.Linq;
    using System.Collections;
    using System.Linq.Expressions;
    using System;

    public interface IUserService
    {
        User FindUser(string userName);
        User FindUser(Func<User, bool> filter);
    }
}