using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using ReactApp.Server.Hubs;
using ReactApp.Server.Models;
using ReactApp.Server.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace ReactApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserConnectionManager _userConnectionManager;

        public ChatController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext,
                              UserConnectionManager userConnectionManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _userConnectionManager = userConnectionManager;
        }

        [HttpGet("getmessages/{recid}")]
        public async Task<IActionResult> GetMessages(string recid)
        {
            var senderid = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var SendMessages = await _dbcontext.Messages
                .Where(m => m.SenderId == senderid && m.ReceiverId == recid)
                .ToListAsync();

            var ReceiveMessages = await _dbcontext.Messages
                .Where(m => m.SenderId == recid && m.ReceiverId == senderid)
                .ToListAsync();

            return Ok(new { SendMessages, ReceiveMessages });
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> getusers()
        {
            var signedInUserId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var usersonline = _userConnectionManager.GetAllConnections();

            var users = await _userManager.Users.Where(u=>u.Id!=signedInUserId).ToListAsync();

            var usersViewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userviewmodel = new UserViewModel
                {
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName,
                    Online = usersonline.Contains(user.Id) ? true : false
                };
                usersViewModel.Add(userviewmodel);
            }

            return Ok(new { usersViewModel });
        }

    }
}
