﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto;
using hefesto.base_hefesto.Services;
using hefesto_dotnet_mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace hefesto_dotnet_mvc.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _logger = logger;
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

        public IActionResult Login()
        {
            LoadMessages();
            return View();
        }

    }
}
