using System.Collections.Generic;
using System.Threading.Tasks;
using udemyApp.API.Models;

namespace udemyApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(int id);
    }
}