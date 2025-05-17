using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    public class AppCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        public int? CategoryId { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; } = 0.00m;

        [StringLength(20)]
        public string Version { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsActive { get; set; } = true;

        public IFormFile? Screenshot { get; set; }

        public SelectList? Categories { get; set; }
    }
}
