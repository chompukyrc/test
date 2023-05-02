using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using web_app_dev_back.Models;
using web_app_dev_back.Services;

namespace web_app_dev_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderSerive;

        private readonly NotificationsService _notificationsService;

        public OrderController(OrderService orderService,NotificationsService notificationsService)
        {
            _orderSerive = orderService;
            _notificationsService = notificationsService;
        }

        [Authorize]
        [HttpGet]
        [Route("List")]
        public async Task<List<OrderModel>> List(string jobId)
        {
            return  await _orderSerive.ListOrderByJobIDAsync(jobId);
        }

        [Authorize]
        [HttpGet]
        [Route("ById")]
        public async Task<OrderModel> Get(string id)
        {
            return await _orderSerive.GetAsync(id);
        }

        [Authorize]
        [HttpPost]
        [Route("Accept")]
        public async Task<IActionResult> AcceptOrder(string id)
        {
            var order = await _orderSerive.GetAsync(id);
            if(order==null){return NotFound();}
            await _notificationsService.CreateAsync(order,"accept");
            await _orderSerive.UpdateStatusAsync(id, "accept");
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Reject")]
        public async Task<IActionResult> RejectOrder(string id)
        {
            var order = await _orderSerive.GetAsync(id);
            if(order==null){return NotFound();}
            await _notificationsService.CreateAsync(order,"reject");
            await _orderSerive.UpdateStatusAsync(id, "reject");
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Done")]
        public async Task<IActionResult> DoneOrder(string id)
        {
            var order = await _orderSerive.GetAsync(id);
            if(order==null){return NotFound();}
            await _notificationsService.CreateAsync(order,"Done");
            await _orderSerive.UpdateStatusAsync(id, "Done");
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(NewOrderModel newOrder)
        {
            string userId = Request.HttpContext.User.FindFirstValue("UserId");

            await _orderSerive.CreateAsync(newOrder, userId);

            return CreatedAtAction("", new { id = "" }, newOrder);
        }

        [Authorize]
        [HttpGet]
        [Route("GetMyOrder")]
        public async Task<List<OrderModel>> GetMyOrder(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            return await _orderSerive.ListOrderByUserIdAsync(userId);
        }
    }
}
