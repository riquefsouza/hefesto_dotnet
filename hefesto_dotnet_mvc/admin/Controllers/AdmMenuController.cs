using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;

namespace hefesto_dotnet_mvc.admin.Controllers
{
    public class AdmMenuController : BaseViewReportController
    {
        private readonly IAdmMenuService _service;

        private readonly IAdmPageService _servicePage;

        private readonly IMessageService _messageService;

        private List<AdmPage> listAdmPage;

        private List<AdmMenu> listAdmMenuParent;

        public AdmMenuController(IAdmMenuService service, IAdmPageService servicePage,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _service = service;
            _servicePage = servicePage;
            _messageService = messageService;

            this.listAdmPage = new List<AdmPage>();
            this.listAdmMenuParent = new List<AdmMenu>();
        }

        private async void FillLists()
        {
            this.listAdmPage = await _servicePage.FindAll();
            ViewData["listAdmPages"] = listAdmPage;

            this.listAdmMenuParent.Clear();

            List<AdmMenu> listAdmMenus = await _service.FindAll();
            foreach (AdmMenu menu in listAdmMenus)
            {
                if ((menu.AdmSubMenus != null) && (menu.AdmPage == null))
                {
                    listAdmMenuParent.Add(menu);
                }
            }

            ViewData["listAdmMenuParents"] = listAdmMenuParent;
        }

        private void filterLists(AdmMenu bean)
        {
            var page = listAdmPage.Where(p => p.Id.Equals(bean.AdmPage.Id)).First();
            if (page!=null)
            {
                bean.AdmPage = page;
                bean.IdPage = page.Id;
            }

            var menuParent = listAdmMenuParent.Where(p => p.Id.Equals(bean.AdmMenuParent.Id)).First();

            if (menuParent != null)
            {
                bean.AdmMenuParent = menuParent;
                bean.IdMenuParent = menuParent.Id;
            }
        }

        public async Task<IActionResult> Index()
        {
            LoadMessages();

            var listAdmMenu = await _service.FindAll();
            return View(listAdmMenu);
        }

        // GET: AdmMenu/Edit/{id}
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FillLists();
            LoadMessages();

            if (id > 0)
            {
                var admMenu = await _service.FindById(id);
                if (admMenu == null)
                {
                    return NotFound();
                }

                filterLists(admMenu);

                return View(admMenu);
            } else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Description,IdMenuParent,IdPage,Order")] AdmMenu admMenu)
        {
            if (id > 0)
            {
                if (id != admMenu.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _service.Update(id, admMenu);
                    if (!updated)
                    {
                        return NotFound();
                    }

                    filterLists(admMenu);

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _service.Insert(admMenu);

                    filterLists(admMenu);

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admMenu);
        }

        // DELETE: AdmMenu/Delete/5
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
