using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbSet<Category> _dbSet;
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<Category>();
            _context = dbContext;
        }

        public async Task CreateAsync(Category model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Category model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsyncWithoutInclude(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
