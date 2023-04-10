using AutoMapper;
using ArdaManager.Application.Exceptions;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Interfaces.Services.Identity;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Infrastructure.Contexts;
using ArdaManager.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Infrastructure.Models.Identity;
using ArdaManager.Shared.Constants.Role;
using Microsoft.Extensions.Localization;
using ArdaManager.Domain.Enums;
using ArdaManager.Domain.Contracts;
using System.Collections;

namespace ArdaManager.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly VappContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ChatService> _localizer;

        public ChatService(
            VappContext context,
            IMapper mapper,
            IUserService userService,
            IStringLocalizer<ChatService> localizer)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _localizer = localizer;
        }

        public async Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => (h.FromUserId == userId && h.ToUserId == contactId) || (h.FromUserId == contactId && h.ToUserId == userId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatHistoryResponse
                    {
                        FromUserId = x.FromUserId,
                        FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
                        ToUserImageURL = x.ToUser.ProfilePictureDataUrl,
                        FromUserImageURL = x.FromUser.ProfilePictureDataUrl
                    }).ToListAsync();
                return await Result<IEnumerable<ChatHistoryResponse>>.SuccessAsync(query);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }

        public async Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId)
        {
            var userRoles = await _userService.GetRolesAsync(userId);
            var userIsAdmin = userRoles.Data?.UserRoles?.Any(x => x.Selected && x.RoleName == RoleConstants.AdministratorRole) == true;
            var allUsers = await _context.Users.Where(user => user.Id != userId && (userIsAdmin || user.IsActive && user.EmailConfirmed)).ToListAsync();
            var chatUsers = _mapper.Map<IEnumerable<ChatUserResponse>>(allUsers);
            return await Result<IEnumerable<ChatUserResponse>>.SuccessAsync(chatUsers);
        }

        public async Task<Result<ChatNotificationCountResponse>> GetUserNotificationCountsAsync(string userId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => (h.ToUserId == userId) && h.MessageType == MessageType.Notification).CountAsync();
                ChatNotificationCountResponse responseData = new ChatNotificationCountResponse() {NotificationCount = query };
                return await Result<ChatNotificationCountResponse>.SuccessAsync(responseData);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }

        public async Task<Result<ChatNotificationCountResponse>> GetUserNotificationUnreadCountsAsync(string userId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => h.ToUserId == userId && h.IsRead == false && h.MessageType == MessageType.Notification).CountAsync();
                ChatNotificationCountResponse responseData = new ChatNotificationCountResponse() { NotificationCount = query };
                return await Result<ChatNotificationCountResponse>.SuccessAsync(responseData);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }

        public async Task<Result<IEnumerable<ChatHistoryResponse>>> GetUserNotificationsAsync(string userId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => h.ToUserId == userId && h.MessageType == MessageType.Notification)
                    .OrderBy(a => a.IsRead)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatHistoryResponse
                    {
                        FromUserId = x.FromUserId,
                        FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
                        ToUserImageURL = x.ToUser.ProfilePictureDataUrl,
                        FromUserImageURL = x.FromUser.ProfilePictureDataUrl,
                        MessageType = x.MessageType,
                        DocType= x.DocType,
                        IsRead=x.IsRead,
                        RelatedDocId = x.RelatedDocId,
                        RelatedLink = x.RelatedLink
                    }).ToListAsync();
                return await Result<IEnumerable<ChatHistoryResponse>>.SuccessAsync(query);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }

        public async Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message)
        {
            try
            {

                message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
                message.FromUser = await _context.Users.Where(user => user.Id == message.FromUserId).FirstOrDefaultAsync();
                await _context.ChatHistories.AddAsync(_mapper.Map<ChatHistory<VappUser>>(message));
                await _context.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                string er = e.Message;
            }
            return await Result.SuccessAsync();
        }

        public async Task<IResult> MarkAsReadAsync(int messageId)
        {
            var message = _context.ChatHistories.FirstOrDefault(x => x.Id == messageId);
            if (message != null)
            {
                message.IsRead = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }
            return await Result.SuccessAsync();
        }
    }
}