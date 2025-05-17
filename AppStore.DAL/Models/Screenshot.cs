using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppStore.DAL.Models
{
    public class Screenshot
    {
        public int Id { get; set; }

        [Required]
        public int AppId { get; set; }

        [ForeignKey("AppId")]
        public App App { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [NotMapped]
        public List<IFormFile>? ImageFile { get; set; }
    }
}
