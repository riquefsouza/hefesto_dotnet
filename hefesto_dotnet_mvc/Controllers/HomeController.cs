using hefesto_dotnet_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Services;

namespace hefesto_dotnet_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAdmProfileService _service;

        public HomeController(ILogger<HomeController> logger, IAdmProfileService service)
        {
            _logger = logger;
            _service = service;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {
            List<long> listaIdProfile = new List<long>();
            listaIdProfile.Add(1);
            listaIdProfile.Add(2);

            return View(await _service.mountMenuItem(listaIdProfile));
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
