using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Repository
{
    public class AppRepository : IAppRepository
    {
        private readonly DbSet<App> _dbSet;
        private readonly AppDbContext _context;

        public AppRepository(AppDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = dbContext.Set<App>();
        }

        public async Task CreateAsync(App model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(App model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<App>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(a => a.Screenshots)
                .Include(a => a.Category)
                .Include(a => a.Developer)
                .ToListAsync();
        }

        public async Task<App?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .Include(a => a.Screenshots)
                .Include(a => a.Category)
                .Include(a => a.Downloads)
                .Include(a => a.Reviews)
                .ThenInclude(x => x.User)
                .Include(a => a.Developer)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<App?> GetByIdWIncludeAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(App model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<App?> GetByIdAsyncWithoutInclude(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<App>> GetAppsByUser(int developerId)
        {
            return await _dbSet.AsNoTracking()
                .Where(a => a.DeveloperId == developerId)
                .Include(a => a.Screenshots)
                .ToListAsync();
        }
    }
}