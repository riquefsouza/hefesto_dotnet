using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;
using Microsoft.AspNetCore.Authorization;

namespace hefesto_dotnet_mvc.admin.Controllers
{
    public class AdmUserController : BaseViewReportController
    {
        private readonly IAdmUserService _service;

        private readonly IMessageService _messageService;

        public AdmUserController(IAdmUserService service,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _service = service;
            _messageService = messageService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            LoadMessages();

            var listAdmUser = await _service.FindAll();
            return View(listAdmUser);
        }

        // GET: AdmUser/Edit/{id}
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoadMessages();

            if (id > 0)
            {
                var admUser = await _service.FindById(id);
                if (admUser == null)
                {
                    return NotFound();
                }
                return View(admUser);
            } else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Active,Email,Login,Name,Password")] AdmUser admUser)
        {
            if (id > 0)
            {
                if (id != admUser.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _service.Update(id, admUser);
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
                    await _service.Insert(admUser);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admUser);
        }

        // DELETE: AdmUser/Delete/5
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
