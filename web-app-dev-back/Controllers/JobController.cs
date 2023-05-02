using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using web_app_dev_back.Models;
using web_app_dev_back.Services;

namespace web_app_dev_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly JobsService _jobsSerive;
        private readonly OrderService _orderService;

        private readonly NotificationsService _notificationsService;

        public JobController(JobsService jobsService, OrderService orderService,NotificationsService notificationsService)
        {
            _jobsSerive = jobsService;
            _orderService = orderService;
            _notificationsService = notificationsService;
        }

        [Authorize]
        [HttpGet]
        [Route("List")]
        public async Task<List<JobModel>> List()
        {
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var status = new List<string>() {"unfinish","close","finish"};
            var jobs = await _jobsSerive.ListAsync(status);
            foreach(var job in jobs)
            {
                var orders = await _orderService.ListOrderByJobIDAsync(job.Id);
                var counter = 0;
                foreach (var order in orders)
                {
                    if (order.Status == "accept")
                    {
                        counter += Convert.ToInt32(order.Count);
                    }
                }
                job.Count = counter;
            };

            return jobs;
        }

        [Authorize]
        [HttpGet]
        [Route("ById")]
        public async Task<JobModel> Get(string id) {
            var job = await _jobsSerive.GetAsync(id);
            var orders = await _orderService.ListOrderByJobIDAsync(job.Id);
            var counter = 0;
            foreach (var order in orders){
                if (order.Status == "accept"){
                    counter += Convert.ToInt32(order.Count);
                }
            }
            job.Count = counter;
            return job;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(NewJobModel newJob)
        {
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            await _jobsSerive.CreateAsync(newJob, userId);

            return CreatedAtAction("", new { id = "" }, newJob);
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateStatusToFinish")]
        public async Task<IActionResult> UpdateStatusToFinish(string id){
            var job = await _jobsSerive.GetAsync(id);
            if (job == null){
                return NotFound();
            }
            job.Status = "finish";
            await _jobsSerive.UpdateAsync(id,job);
            var acceptOrders = await _orderService.ListStatusByJobIDAsync(id,"accept");
            foreach (var acceptOrder in acceptOrders){
                await _notificationsService.CreateAsync(acceptOrder,"Done");
                await _orderService.UpdateStatusAsync(acceptOrder.Id, "Done");
            }
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateStatusToClose")]
        public async Task<IActionResult> UpdateStatusToClose(string id){
            var job = await _jobsSerive.GetAsync(id);
            if (job == null){
                return NotFound();
            }
            job.Status = "close";
            await _jobsSerive.UpdateAsync(id,job);
            var waitingOrders = await _orderService.ListStatusByJobIDAsync(id,"waiting");
            foreach (var waitingOrder in waitingOrders){
                await _notificationsService.CreateAsync(waitingOrder,"reject");
                await _orderService.UpdateStatusAsync(waitingOrder.Id, "reject");
            }
            return Ok();
        }
    }
}