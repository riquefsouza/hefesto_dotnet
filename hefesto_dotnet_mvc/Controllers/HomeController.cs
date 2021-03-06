using hefesto_dotnet_mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.VO;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Util;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace hefesto_dotnet_mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ISystemService systemService;

        public HomeController(ILogger<HomeController> logger,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
            this.systemService = systemService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            LoadMessages();

            var authenticatedUser = this.GetAuthenticatedUser();

            if (authenticatedUser != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }
        
    }
}
