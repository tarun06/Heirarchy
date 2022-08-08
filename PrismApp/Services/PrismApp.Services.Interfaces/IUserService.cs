using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrismApp.Services.Interfaces
{
    public interface IUserService<T>
    {
        Task<IEnumerable<T>> GetUsers(string url);

        Task<IEnumerable<T>> GetUsers(Uri url);
    }
}