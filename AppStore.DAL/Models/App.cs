using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppStore.DAL.Models
{
    public class App
    {
        public int Id { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        [ForeignKey("DeveloperId")]
        public User Developer { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; } = 0.00m;

        [StringLength(20)]
        public string Version { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Download> Downloads { get; set; } = new List<Download>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Screenshot> Screenshots { get; set; } = new List<Screenshot>();
    }
}
