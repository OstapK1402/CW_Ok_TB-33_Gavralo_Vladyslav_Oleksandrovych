using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Repository
{
    public class ScreenshotRepository : IScreenshotRepository
    {
        private readonly DbSet<Screenshot> _dbSet;
        private readonly AppDbContext _context;

        public ScreenshotRepository(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<Screenshot>();
            _context = dbContext;
        }

        public async Task CreateAsync(Screenshot model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Screenshot model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Screenshot>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<Screenshot?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Screenshot model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Screenshot?> GetByIdAsyncWithoutInclude(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
