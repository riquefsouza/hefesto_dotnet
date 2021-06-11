using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Models;
using hefesto.base_hefesto.Services;
using hefesto.admin.Models;
using hefesto.admin.VO;
using hefesto.admin.Services;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace hefesto_dotnet_mvc.Controllers
{
    public class ChangePasswordEditController : BaseController
    {
        private readonly ILogger<ChangePasswordEditController> _logger;

        private readonly IAdmUserService _service;

        private readonly IMessageService _messageService;

        private readonly IChangePasswordService _changePasswordService;

        private UserVO userLogged;

        private AlertMessageVO alertMessage;

        public ChangePasswordEditController(ILogger<ChangePasswordEditController> logger, IAdmUserService service,
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

            userLogged = this.GetAuthenticatedUser().User;

            return View(userLogged);
        }

        public bool PrepareToSave(UserVO user)
        {
            if ((user.NewPassword == null && user.ConfirmNewPassword == null && user.CurrentPassword == null)
                    || (user.NewPassword.Equals("") && user.ConfirmNewPassword.Equals("") && user.CurrentPassword.Equals("")))
            {
                alertMessage = AlertMessageVO.Warning(_messageService, "changePasswordView.validation");
            }
            else if ((user.NewPassword == null && user.ConfirmNewPassword == null)
                  || (user.NewPassword.Equals("") && user.ConfirmNewPassword.Equals("")))
            {
                alertMessage = AlertMessageVO.Warning(_messageService, "changePasswordView.validation");
            }
            else
            {
                if (user.NewPassword.Equals(user.ConfirmNewPassword))
                {
                    return true;
                }
                else
                {
                    alertMessage = AlertMessageVO.Warning(_messageService, "changePasswordView.notEqual");
                }
            }
            return false;
        }

        public bool ValidatePassword(UserVO user)
        {
            if (!this._changePasswordService.ValidatePassword(user.Login, user.CurrentPassword))
            {
                alertMessage = AlertMessageVO.Warning(_messageService, "changePasswordView.validatePassword");
                return false;
            }

            if (!this._changePasswordService.ValidatePassword(user.Login, user.NewPassword))
            {
                alertMessage = AlertMessageVO.Warning(_messageService, "changePasswordView.validatePassword");
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Active,Email,Login,Name,Password,CurrentPassword,NewPassword,ConfirmNewPassword")] UserVO user)
        {
            if (!PrepareToSave(user))
            {
                LoadMessages(alertMessage);
                return View(nameof(Index), user);
            }

            if (!ValidatePassword(user))
            {
                LoadMessages(alertMessage);
                return View(nameof(Index), user);
            }

            if (id > 0)
            {
                if (id != user.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    string pwdCrypt = BC.HashPassword(user.ConfirmNewPassword, BC.GenerateSalt(10));
                    
                    userLogged.Password = pwdCrypt;
                    AdmUser admUser = new AdmUser(userLogged);                   

                    var updated = await _service.Update(id, admUser);
                    if (!updated)
                    {
                        return NotFound();
                    }

                    alertMessage = AlertMessageVO.Info(_messageService, "changePasswordView.passwordChanged");
                    LoadMessages(alertMessage);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
