using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Repository
{
    public class ReviewRepository : IReviewRepository
    { 
        private readonly DbSet<Review> _dbSet;
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<Review>();
            _context = dbContext;
        }

        public async Task CreateAsync(Review model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Review model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Review model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Review?> GetByIdAsyncWithoutInclude(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Review>?> GetReviewsByUserAsync(int userId)
        {
            return await _dbSet.AsNoTracking()
                .Where(r => r.UserId == userId)
                .Include(r => r.App)
                .ToListAsync();
        }
    }
}
