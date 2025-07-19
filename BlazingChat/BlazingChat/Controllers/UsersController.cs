using BlazingChat.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlazingChat.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly ChatContext _chatContext;

        public UsersController(ChatContext chatContext)
        {
            _chatContext = chatContext;

        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetUsers() =>
            await _chatContext.Users
                        .AsNoTracking()
                        .Where(u => u.Id != UserId)
                        .Select(u => new UserDto(u.Id, u.Name, false))
                        .ToListAsync();

        [HttpGet("chats")]
        public async Task<IEnumerable<UserDto>> GetUserChats(CancellationToken cancellationToken)
        {
            var uniqueUserIds = await _chatContext.Messages
                .AsNoTracking()
                .Where(m => m.FromId == UserId || m.ToId == UserId)
                .Select(m => m.FromId == UserId ? m.ToId : m.FromId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var chatUsers = await _chatContext.Users
                .AsNoTracking()
                .Where(u => uniqueUserIds.Contains(u.Id))
                .Select(u => new UserDto(u.Id, u.Name, false))
                .ToListAsync(cancellationToken);

            return chatUsers;
        }

    }
}
