using AppStore.DAL.Models;

namespace AppStore.DAL.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task CreateAsync(Category model);
        Task Update(Category model);
        Task Delete(Category model);
    }
}
