using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;

namespace hefesto_dotnet_mvc.Controllers
{
    public class AdmParameterController : BaseViewReportController
    {
        private readonly IAdmParameterService _service;

        private readonly IAdmParameterCategoryService _serviceParameterCategory;

        private readonly IMessageService _messageService;

        public AdmParameterController(IAdmParameterService service,
            IAdmParameterCategoryService serviceParameterCategory,
            IMessageService messageService): base(messageService)
        {
            _service = service;
            _serviceParameterCategory = serviceParameterCategory;
            _messageService = messageService;
        }

        private async void LoadAdmParameterCategory()
        {
            var listAdmCategories = await _serviceParameterCategory.FindAll();
            ViewData["listAdmCategories"] = listAdmCategories;
            //ViewBag.listAdmCategories = listAdmCategories;
        }

        public async Task<IActionResult> Index()
        {
            LoadMessages();

            var listAdmParameter = await _service.FindAll();
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
                var admParameter = await _service.FindById(id);
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

        // DELETE: AdmParameter/Delete/5
        //[HttpDelete, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<ActionResult> Delete(long id)
        {
            await _service.Delete(id);
            return new JsonResult(new object());
        }

    }

}
