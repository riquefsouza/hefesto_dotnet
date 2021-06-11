using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;
using hefesto.admin.VO;
using Microsoft.AspNetCore.Mvc;

namespace hefesto_dotnet_mvc.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        private UserVO userLogged;

        public LoginController(ILogger<LoginController> logger,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            LoadMessages();

            userLogged = this.GetAuthenticatedUser().User;

            return View(userLogged);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(long id, 
            [Bind("Id,Active,Email,Login,Name,Password,CurrentPassword,NewPassword,ConfirmNewPassword")] UserVO user)
        {            

            if (user.Login.Trim().Length > 0 && user.Password.Trim().Length > 0)
            {

            }

            return View("/home");
        }

    }
}
