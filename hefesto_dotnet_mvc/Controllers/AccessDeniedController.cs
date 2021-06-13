using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;
using Microsoft.AspNetCore.Mvc;

namespace hefesto_dotnet_mvc.Controllers
{
    public class AccessDeniedController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        //public string? ReturnUrl { get; set; }

        //public string ReturnUrlParameter { get; set; }

        public AccessDeniedController(ILogger<LoginController> logger,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/Account/AccessDenied")]
        public IActionResult Index()
        {
            LoadMessages();

            return View();
        }

    }
}
