using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppStore.DAL.Models
{
    public class Download
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int AppId { get; set; }

        [ForeignKey("AppId")]
        public App App { get; set; }

        [Required]
        public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;
    }
}
