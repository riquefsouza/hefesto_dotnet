using hefesto_dotnet_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Services;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;

namespace hefesto_dotnet_mvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            LoadMessages();

            /*
            get
            {
                object value = HttpContext.Current.Session["TestSessionValue"];
                return value == null ? "" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["TestSessionValue"] = value;
            }
            */

            return View();
        }


    }
}
