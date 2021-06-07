using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Models;

namespace hefesto.base_hefesto
{
    public class BaseController : Controller
    {
        protected IMessageService MessageService { get; set; }

        private readonly ISystemService _systemService;

        public BaseController(IMessageService messageService, ISystemService systemService)
        {
            this.MessageService = messageService;
            _systemService = systemService;
        }

        virtual async protected void LoadMessages()
        {
            ViewData["AlertMessage"] = new AlertMessageVO();

            foreach (var msg in MessageService.GetMessages())
            {
                ViewData[msg.Key] = msg.Value;
            }

            List<long> listaIdProfile = new List<long>();
            listaIdProfile.Add(1);
            listaIdProfile.Add(2);

            ViewData["MenuItem"] = await _systemService.MountMenuItem(listaIdProfile);

        }

    }
}
