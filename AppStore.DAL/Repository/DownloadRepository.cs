using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Repository
{
    public class DownloadRepository : IDownloadRepository
    {
        private readonly DbSet<Download> _dbSet;
        private readonly AppDbContext _context;

        public DownloadRepository(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<Download>();
            _context = dbContext;
        }

        public async Task CreateAsync(Download model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Download model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Download>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<Download?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Download model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Download?> GetByIdAsyncWithoutInclude(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
