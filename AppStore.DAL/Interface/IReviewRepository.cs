using AppStore.DAL.Models;

namespace AppStore.DAL.Interface
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task CreateAsync(Review model);
        Task Update(Review model);
        Task Delete(Review model);
        Task<IEnumerable<Review>?> GetReviewsByUserAsync(int userId);

    }
}
