using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        { }

        public string Description { get; set; }
    }
}