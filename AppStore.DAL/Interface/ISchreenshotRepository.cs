using AppStore.DAL.Models;

namespace AppStore.DAL.Interface
{
    public interface IScreenshotRepository
    {
        Task<IEnumerable<Screenshot>> GetAllAsync();
        Task<Screenshot?> GetByIdAsync(int id);
        Task CreateAsync(Screenshot model);
        Task Update(Screenshot model);
        Task Delete(Screenshot model);
    }
}
