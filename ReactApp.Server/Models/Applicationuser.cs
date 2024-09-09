using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public bool online { get; set; }

    }
}
