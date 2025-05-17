namespace AppStore.DAL.Interface
{
    public interface IUnitOfWork
    {
        IAppRepository AppRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDownloadRepository DownloadRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IScreenshotRepository ScreenshotRepository { get; }
        IUserRepository UserRepository { get; }

        void Dispose();
        Task<int> SaveAsync();
    }
}
