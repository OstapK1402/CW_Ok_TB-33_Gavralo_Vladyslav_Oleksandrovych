using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models
{
    public class AppEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string Version { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsActive { get; set; }

        public SelectList? Categories { get; set; }
       
    }
}
