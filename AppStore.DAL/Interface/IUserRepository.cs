using AppStore.DAL.Models;

namespace AppStore.DAL.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task CreateAsync(User model);
        Task Update(User model);
        Task Delete(User model);
    }
}
