using AppStore.DAL.Context;
using AppStore.DAL.Interface;
using AppStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _dbSet;
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<User>();
            _context = dbContext;
        }

        public async Task CreateAsync(User model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(User model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(User model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
