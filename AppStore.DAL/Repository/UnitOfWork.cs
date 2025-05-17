using AppStore.DAL.Context;
using AppStore.DAL.Interface;

namespace AppStore.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IAppRepository AppRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IDownloadRepository DownloadRepository  { get; private set; }
        public IReviewRepository ReviewRepository { get; private set; }
        public IScreenshotRepository ScreenshotRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public UnitOfWork(AppDbContext dbContext, IAppRepository appRepository, ICategoryRepository categoryRepository, IDownloadRepository downloadRepository,
            IReviewRepository reviewRepository, IScreenshotRepository screenshotRepository, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            AppRepository = appRepository;
            CategoryRepository = categoryRepository;
            DownloadRepository = downloadRepository;
            ReviewRepository = reviewRepository;
            ScreenshotRepository = screenshotRepository;
            UserRepository = userRepository;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
