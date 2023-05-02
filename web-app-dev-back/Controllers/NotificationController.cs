using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using web_app_dev_back.Models;
using web_app_dev_back.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace web_app_dev_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationsService _notificationsService;

        public NotificationController(NotificationsService notificationsService) =>
            _notificationsService = notificationsService;

        [Authorize]
        [HttpGet]
        [Route("ListByUserId")]
        public async Task<List<NotificationModel>> ListByUserId(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            return await _notificationsService.ListByUserIdAsync(userId);
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateToRead")]
        public async Task<IActionResult> UpdateToRead(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var unreadNotiList = await _notificationsService.ListUnreadAsync(userId);
            foreach(var unreadNoti in unreadNotiList){
                unreadNoti.isRead = true;
                await _notificationsService.UpdateAsync(unreadNoti.Id,unreadNoti);
            }
            return Ok();
        }
    }
}