using Microsoft.AspNetCore.Identity;

namespace BlogAppAPI.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Comment> Comments { get; set; }

    }
}
