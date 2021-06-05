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

        private readonly IAdmParameterCategoryService _serviceParameterCategory;

        public readonly IConfiguration _configuration;

        private List<IConfigurationSection> _messages;

        private readonly IHtmlLocalizer<AdmParameterController> _localizer;

        public AdmParameterController(dbhefestoContext context, IAdmParameterService service,
            IAdmParameterCategoryService serviceParameterCategory,
            IConfiguration configuration, IHtmlLocalizer<AdmParameterController> localizer)
        {
            _context = context;
            _service = service;
            _serviceParameterCategory = serviceParameterCategory;
            _localizer = localizer;
            _configuration = configuration;

            _messages = _configuration.GetSection("Messages_pt_BR").GetChildren().ToList();
        }

        private void LoadMessages()
        {
            foreach (var msg in _messages)
            {
                ViewData[msg.Key] = msg.Value;
            }

        }

        private async void LoadAdmParameterCategory()
        {
            var listAdmCategories = await _serviceParameterCategory.FindAll();
            ViewData["listAdmCategories"] = listAdmCategories;
        }

        public async Task<IActionResult> Index()
        {
            LoadMessages();

            var listAdmParameter = await _service.FindAll();
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

            LoadAdmParameterCategory();
            LoadMessages();

            if (id > 0)
            {
                var admParameter = await _context.AdmParameters.FindAsync(id);
                if (admParameter == null)
                {
                    return NotFound();
                }
                return View(admParameter);
            } else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Code,Description,IdParameterCategory,Value")] AdmParameter admParameter)
        {
            if (id > 0)
            {
                if (id != admParameter.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _service.Update(id, admParameter);
                    if (!updated)
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _service.Insert(admParameter);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admParameter);
        }
    }

}
