using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Year { get; set; }
        
        public ApplicationUser()
        {
        }
    }
}