using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ReactApp.Server.Data;
using ReactApp.Server.Models;

namespace ReactApp.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbcontext;

        public ChatHub(UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }


    }
}
