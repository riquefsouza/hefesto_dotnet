using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;
using hefesto.admin.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;

namespace hefesto_dotnet_mvc.Controllers
{
    public class AdmParameterController : Controller
    {        
        private readonly dbhefestoContext _context;

        private readonly IAdmParameterService _service;

        public readonly IConfiguration _configuration;

        private List<IConfigurationSection> _messages;

        private readonly IHtmlLocalizer<AdmParameterController> _localizer;

        public AdmParameterController(dbhefestoContext context, IAdmParameterService service,
            IConfiguration configuration, IHtmlLocalizer<AdmParameterController> localizer)
        {
            _context = context;
            _service = service;
            _localizer = localizer;
            _configuration = configuration;

            _messages = _configuration.GetSection("Messages_pt_BR").GetChildren().ToList();
        }

        public async Task<IActionResult> ListAdmParameter()
        {
            foreach (var msg in _messages)
            {
                ViewData[msg.Key] = msg.Value;
            }

            var listAdmParameter = await _context.AdmParameters.ToListAsync();
            _service.SetTransient(listAdmParameter);
            return View(listAdmParameter);
        }

        // GET: AdmParameter/Edit/{id}
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admParameter = await _context.AdmParameters.FindAsync(id);
            if (admParameter == null)
            {
                return NotFound();
            }
            return View(admParameter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, [Bind("Id,Code,Description,IdParameterCategory,Value")] AdmParameter admParameter)
        {
            if (id != admParameter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ListAdmParameter));
            }

            return View(admParameter);
        }
    }
