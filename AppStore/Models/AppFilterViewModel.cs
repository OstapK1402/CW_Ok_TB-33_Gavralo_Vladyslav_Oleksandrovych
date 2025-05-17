using AppStore.DAL.Models;

namespace AppStore.Models
{
    public class AppFilterViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int? CategoryId { get; set; }

        public List<Category> Categories { get; set; } = new();
        public List<App> Apps { get; set; } = new();
    }
}
