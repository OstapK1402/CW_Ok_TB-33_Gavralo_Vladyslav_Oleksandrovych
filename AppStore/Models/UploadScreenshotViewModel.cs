using AppStore.DAL.Models;

namespace AppStore.Models
{
    public class UploadScreenshotViewModel
    {
        public int AppId { get; set; }

        public App? App { get; set; }

        public List<IFormFile> ImageFile { get; set; } = new();
    }
}
