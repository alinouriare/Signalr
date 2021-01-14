using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Sgnal.Hubs;
using Sgnal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sgnal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public HomeController(IHubContext<NotificationHub> chatHubContext)
        {
            _hubContext = chatHubContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChatToAll()
        {
            return View();
        }
        #region notif
        public IActionResult NotificationToAll()
        {
            return View();
        }
        public async Task<ActionResult> SendNotification(string msg)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", msg);

            return RedirectToAction("NotificationToAll");
        }
        #endregion
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChatToGroup()
        {
            return View();
        }

        public IActionResult ChatP2P()
        {
            return View();
        }
    }
}
