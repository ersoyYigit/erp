using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Shared.Constants.Permission;

namespace ArdaManager.Server.Controllers.Communication
{
    
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChatsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IChatService _chatService;

        public ChatsController(ICurrentUserService currentUserService, IChatService chatService)
        {
            _currentUserService = currentUserService;
            _chatService = chatService;
        }

        /// <summary>
        /// Get user wise chat history
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("GetChatHistoryAsync/{contactId}")]
        public async Task<IActionResult> GetChatHistoryAsync(string contactId)
        {
            return Ok(await _chatService.GetChatHistoryAsync(_currentUserService.UserId, contactId));
        }
        /// <summary>
        /// get available users
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //get available users - sorted by date of last message if exists
        [HttpGet("GetChatUsersAsync")]
        public async Task<IActionResult> GetChatUsersAsync()
        {
            return Ok(await _chatService.GetChatUsersAsync(_currentUserService.UserId));
        }

        /// <summary>
        /// Save Chat Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Status 200 OK</returns>
        //save chat message
        [HttpPost("SaveMessageAsync")]
        public async Task<IActionResult> SaveMessageAsync(ChatHistory<IChatUser> message)
        {
            message.FromUserId = _currentUserService.UserId;
            message.ToUserId = message.ToUserId;
            message.CreatedDate = DateTime.Now;
            return Ok(await _chatService.SaveMessageAsync(message));
        }

        /// <summary>
        /// Mark as read 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>Status 200 OK</returns>
        //save chat message
        [HttpPost("MarkAsReadAsync")]
        public async Task<IActionResult> MarkAsReadAsync(int messageId)
        {
            
            return Ok(await _chatService.MarkAsReadAsync(messageId));
        }


        /// <summary>
        /// Get user notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("GetUserNotificationsAsync")]
        public async Task<IActionResult> GetUserNotificationsAsync()
        {
            return Ok(await _chatService.GetUserNotificationsAsync(_currentUserService.UserId));
        }


        /// <summary>
        /// Get user notification count
        /// </summary>

        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("GetUserNotificationCountsAsync")]
        public async Task<IActionResult> GetUserNotificationCountsAsync()
        {
            return Ok(await _chatService.GetUserNotificationCountsAsync(_currentUserService.UserId));
        }

        /// <summary>
        /// Get user unread notification count
        /// </summary>

        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("GetUserNotificationUnreadCountsAsync")]
        public async Task<IActionResult> GetUserNotificationUnreadCountsAsync()
        {
            return Ok(await _chatService.GetUserNotificationUnreadCountsAsync(_currentUserService.UserId));
        }
    }
}