using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;
using Microsoft.AspNetCore.Authorization;

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

        private async Task<bool> FillLists()
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

            return true;
        }

        private void FilterLists(AdmMenu bean)
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

        [Authorize]
        public IActionResult Index()
        {
            LoadMessages();

            //var listAdmMenu = await _service.FindAll();
            var listAdmMenu = GetAuthenticatedUser().ListAdminMenus;

            return View(listAdmMenu);
        }

        // GET: AdmMenu/Edit/{id}
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await FillLists();
            LoadMessages();

            if (id > 0)
            {
                var admMenu = await _service.FindById(id);
                if (admMenu == null)
                {
                    return NotFound();
                }

                FilterLists(admMenu);

                return View(admMenu);
            } else
            {
                var admMenu = new AdmMenu();

                return View(admMenu);
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

                    FilterLists(admMenu);

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _service.Insert(admMenu);

                    FilterLists(admMenu);

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
