using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;
using hefesto.admin.VO;
using Microsoft.AspNetCore.Mvc;
using hefesto.base_hefesto.Util;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace hefesto_dotnet_mvc.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        private readonly IMessageService messageService;

        private readonly ISystemService systemService;

        private UserVO userLogged;

        public LoginController(ILogger<LoginController> logger,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
            this.messageService = messageService;
            this.systemService = systemService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoadMessages();

            userLogged = new UserVO();

            ViewData["loginError"] = false;

            return View(userLogged);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Active,Email,Login,Name,Password")] UserVO user)
        {
            if (user.Login.Trim().Length > 0 && user.Password.Trim().Length > 0)
            {

                if (systemService.Authenticate(user))
                {

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Login));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, user.Name));

                    foreach (var role in user.AdmIdProfiles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    //HttpContext.Session.SetString("authenticatedUser", systemService.GetAuthenticatedUser());
                    var authenticatedUser = systemService.GetAuthenticatedUser();
                    HttpContext.Session.Set<AuthenticatedUserVO>("authenticatedUser", authenticatedUser);
                } 
                else
                {
                    LoadMessages();
                    ViewData["loginError"] = true;
                    ModelState.AddModelError("", messageService.GetMessage("login.wrongUser"));
                    return View(user);
                }
            }
            else
            {
                LoadMessages();
                ViewData["loginError"] = true;
                ModelState.AddModelError("", messageService.GetMessage("login.wrongUser"));
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            RemoveUserAuthenticated();
            LoadMessages();

            //return RedirectToAction("Login", "Login");
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "AccessDenied");
        }

    }
}
