using System.ComponentModel.DataAnnotations;

namespace AppStore.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<App> Apps { get; set; } = new List<App>();
    }
}
