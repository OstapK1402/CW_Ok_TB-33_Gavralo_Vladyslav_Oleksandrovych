using AppStore.DAL.Models;

namespace AppStore.DAL.Interface
{
    public interface IAppRepository
    {
        Task<IEnumerable<App>> GetAllAsync();
        Task<App?> GetByIdAsync(int id);
        Task CreateAsync(App model);
        Task Update(App model);
        Task Delete(App model);
        Task<IEnumerable<App>> GetAppsByUser(int developerId);
        Task<App?> GetByIdWIncludeAsync(int id);
    }
}
