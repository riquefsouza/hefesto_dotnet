using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.VO;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Models;
using hefesto.base_hefesto.Util;

namespace hefesto.base_hefesto
{
    public class BaseController : Controller
    {
        protected readonly IMessageService MessageService;

        private readonly ISystemService _systemService;

        public BaseController(IMessageService messageService, ISystemService systemService)
        {
            this.MessageService = messageService;
            _systemService = systemService;
        }

        virtual protected void LoadMessages()
        {
            this.LoadMessages(null);
        }

        virtual protected void LoadMessages(AlertMessageVO alertMessage)
        {
            if (alertMessage == null)
                ViewData["AlertMessage"] = new AlertMessageVO();
            else
                ViewData["AlertMessage"] = alertMessage;

            foreach (var msg in MessageService.GetMessages())
            {
                ViewData[msg.Key] = msg.Value;
            }

            //List<long> listaIdProfile = new List<long>();
            //listaIdProfile.Add(1);
            //listaIdProfile.Add(2);
            //ViewData["MenuItem"] = await _systemService.MountMenuItem(listaIdProfile);

            var authenticatedUser = this.GetAuthenticatedUser();

            var listMenus = authenticatedUser.ListAdminMenus;
            
            ViewData["MenuItem"] = listMenus;
        }

        public AuthenticatedUserVO GetAuthenticatedUser()
        {
            return HttpContext.Session.Get<AuthenticatedUserVO>("authenticatedUser");
        }

        public void SetUserAuthenticated(AuthenticatedUserVO usu)
        {
            HttpContext.Session.Set<AuthenticatedUserVO>("authenticatedUser", usu);
        }

    }
}
