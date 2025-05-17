using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppStore.DAL.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public ICollection<App> Apps { get; set; } = new List<App>();
        public ICollection<Download> Downloads { get; set; } = new List<Download>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
