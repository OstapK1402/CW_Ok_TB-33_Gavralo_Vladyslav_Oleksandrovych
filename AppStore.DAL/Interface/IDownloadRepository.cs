using AppStore.DAL.Models;

namespace AppStore.DAL.Interface
{
    public interface IDownloadRepository
    {
        Task<IEnumerable<Download>> GetAllAsync();
        Task<Download?> GetByIdAsync(int id);
        Task CreateAsync(Download model);
        Task Update(Download model);
        Task Delete(Download model);
    }
}
