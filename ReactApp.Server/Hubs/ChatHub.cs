using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReactApp.Server.Data;
using ReactApp.Server.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ReactApp.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserConnectionManager _userConnectionManager;

        public ChatHub(UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext,
                      UserConnectionManager userConnectionManager)
        {
            _botUser = "MyChat_Bot";
            _dbcontext = dbcontext;
            _userManager = userManager;
            _userConnectionManager = userConnectionManager;
        }

        public override Task OnConnectedAsync()
        {
            var userid = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value; // Assume the user is authenticated
            var connectionId = Context.ConnectionId;
            _userConnectionManager.AddConnection(userid, connectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveConnection(connectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessagePrivate(string recid, string message)
        {
            var RecConnectionId = _userConnectionManager.GetConnection(recid);
            var senderId = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;


            if (RecConnectionId != null)
            {
                await Clients.Client(RecConnectionId).SendAsync("ReceiveMessagePrivate", message);
            }

            var messagetodb = new Message
            {
                SenderId = senderId,
                ReceiverId = recid,
                Content = message
            };

            await _dbcontext.AddAsync(messagetodb);
            await _dbcontext.SaveChangesAsync();

        }


        ////// room //////////////
        
        /*public async Task JoinRoom(string Room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Room);

            _connections[Context.ConnectionId] = userConnection;

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            await SendUsersConnected(userConnection.Room);
        }*/



    }
}
