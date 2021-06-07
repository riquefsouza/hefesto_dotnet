using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;
using hefesto.base_hefesto.Pagination;

namespace hefesto_dotnet_mvc.Controllers
{
    public class AdmParameterCategoryController : BaseViewReportController
    {
        private readonly IAdmParameterCategoryService _service;

        private readonly IMessageService _messageService;

        public AdmParameterCategoryController(IAdmParameterCategoryService service,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _service = service;
            _messageService = messageService;
        }

        public async Task<IActionResult> Index([FromQuery] PaginationFilter filter)
        {
            LoadMessages();

            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            return View(pagedData);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoadMessages();

            if (id > 0)
            {
                var admParameterCategory = await _service.FindById(id);
                if (admParameterCategory == null)
                {
                    return NotFound();
                }
                return View(admParameterCategory);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Description,Order")] AdmParameterCategory admParameterCategory)
        {
            if (id > 0)
            {
                if (id != admParameterCategory.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _service.Update(id, admParameterCategory);
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
                    await _service.Insert(admParameterCategory);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admParameterCategory);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            await _service.Delete(id);
            return new JsonResult(new object());
        }

    }
}
