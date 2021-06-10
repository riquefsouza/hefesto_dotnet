using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;
using hefesto.admin.Models;
using hefesto.admin.Services;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace hefesto_dotnet_mvc.Controllers
{
    public class ChangePasswordController : BaseController
    {
        private readonly ILogger<ChangePasswordController> _logger;

        private readonly IAdmUserService _service;

        private readonly IMessageService _messageService;

        private readonly IChangePasswordService _changePasswordService;

        public ChangePasswordController(ILogger<ChangePasswordController> logger, IAdmUserService service,
            IChangePasswordService changePasswordService,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
            _service = service;
            _messageService = messageService;
            _changePasswordService = changePasswordService;
        }

        public IActionResult Index()
        {
            LoadMessages();

            //var userLogged = userDetails.getAuthenticatedUser().getUser();

            //return View(userLogged);
            return View();
        }

        public bool prepararParaSalvar(AdmUser user)
        {
            if ((user.NewPassword == null && user.ConfirmNewPassword == null && user.CurrentPassword == null)
                    || (user.NewPassword.Equals("") && user.ConfirmNewPassword.Equals("") && user.CurrentPassword.Equals("")))
            {
                //this.showWarningMessage(mv, "changePasswordView.validation");
            }
            else if ((user.NewPassword == null && user.ConfirmNewPassword == null)
                  || (user.NewPassword.Equals("") && user.ConfirmNewPassword.Equals("")))
            {
                //this.showWarningMessage(mv, "changePasswordView.validation");
            }
            else
            {
                if (user.NewPassword.Equals(user.ConfirmNewPassword))
                {
                    return true;
                }
                else
                {
                    //this.showWarningMessage(mv, "changePasswordView.notEqual");
                }
            }
            return false;
        }

        public bool validatePassword(AdmUser user)
        {
            if (!this._changePasswordService.ValidatePassword(user.Login, user.CurrentPassword))
            {
                //this.showWarningMessage(mv, "changePasswordView.validatePassword");
                return false;
            }

            if (!this._changePasswordService.ValidatePassword(user.Login, user.NewPassword))
            {
                //this.showWarningMessage(mv, "changePasswordView.validatePassword");
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Active,Email,Login,Name,Password,CurrentPassword,NewPassword,ConfirmNewPassword")] AdmUser admUser)
        {
            if (!prepararParaSalvar(admUser))
            {
                return View(admUser);
            }

            if (!validatePassword(admUser))
            {
                return View(admUser);
            }

            if (id > 0)
            {
                if (id != admUser.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    string pwdCrypt = BC.HashPassword(admUser.ConfirmNewPassword, BC.GenerateSalt(10));
                    admUser.Password = pwdCrypt;
                    //userLogged.setPassword(pwdCrypt);
                    //AdmUser admUser = new AdmUser(userLogged);                   

                    var updated = await _service.Update(id, admUser);
                    if (!updated)
                    {
                        return NotFound();
                    }

                    //this.showWarningMessage(mv.get(), "changePasswordView.passwordChanged");

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admUser);
        }
    }
}
