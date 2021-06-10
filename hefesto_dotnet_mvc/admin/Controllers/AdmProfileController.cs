using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;
using hefesto.base_hefesto;

namespace hefesto_dotnet_mvc.admin.Controllers
{
    public class AdmProfileController : BaseViewReportController
    {
        private readonly IMessageService _messageService;

        private readonly IAdmProfileService _service;

        private readonly IAdmPageService admPageService;

        private BaseDualList<AdmPage> dualListAdmPage;

        private List<AdmPage> listAllAdmPages;

        private IAdmUserService admUserService;

        private BaseDualList<AdmUser> dualListAdmUser;

        private List<AdmUser> listAllAdmUsers;

        public AdmProfileController(IAdmProfileService service, 
            IAdmPageService admPageService, IAdmUserService admUserService,
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _service = service;            
            _messageService = messageService;
            this.admPageService = admPageService;
            this.admUserService = admUserService;

            this.listAllAdmPages = new List<AdmPage>();
            this.listAllAdmUsers = new List<AdmUser>();
        }

        private async Task<BaseDualList<AdmPage>> loadAdmPages(AdmProfile bean, bool bEdit)
        {
            List<AdmPage> listAdmPagesSelected;
            List<AdmPage> listAdmPages = await admPageService.FindAll();

            listAllAdmPages.Clear();
            listAllAdmPages.AddRange(listAdmPages);

            if (bEdit)
            {
                listAdmPagesSelected = new List<AdmPage>();

                foreach (AdmPage page in listAdmPages)
                {
                    foreach (long profileId in page.AdmIdProfiles)
                    {
                        if (profileId.Equals(bean.Id))
                        {
                            listAdmPagesSelected.Add(page);
                            break;
                        }
                    }
                }

                listAdmPages.RemoveAll(item => listAdmPagesSelected.Contains(item));
            }
            else
            {
                listAdmPagesSelected = new List<AdmPage>();
            }

            this.dualListAdmPage = new BaseDualList<AdmPage>(listAdmPages, listAdmPagesSelected);

            return this.dualListAdmPage;
        }

        private async Task<BaseDualList<AdmUser>> loadAdmUsers(AdmProfile bean, bool bEdit)
        {
            List<AdmUser> listAdmUsersSelected;
            List<AdmUser> listAdmUsers = await admUserService.FindAll();

            listAllAdmUsers.Clear();
            listAllAdmUsers.AddRange(listAdmUsers);

            if (bEdit)
            {
                listAdmUsersSelected = new List<AdmUser>();

                foreach (AdmUser user in listAdmUsers)
                {
                    foreach (long profileId in user.AdmIdProfiles)
                    {
                        if (profileId.Equals(bean.Id))
                        {
                            listAdmUsersSelected.Add(user);
                            break;
                        }
                    }
                }

                listAdmUsers.RemoveAll(item => listAdmUsersSelected.Contains(item));
            }
            else
            {
                listAdmUsersSelected = new List<AdmUser>();
            }

            this.dualListAdmUser = new BaseDualList<AdmUser>(listAdmUsers, listAdmUsersSelected);

            return this.dualListAdmUser;
        }

        public async Task<IActionResult> Index()
        {
            LoadMessages();

            var listAdmProfile = await _service.FindAll();
            return View(listAdmProfile);
        }

        // GET: AdmProfile/Edit/{id}
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoadMessages();

            if (id > 0)
            {
                var admProfile = await _service.FindById(id);
                if (admProfile == null)
                {
                    return NotFound();
                }

                this.dualListAdmPage = await loadAdmPages(admProfile, true);
                ViewData["listSourceAdmPages"] = this.dualListAdmPage.Source;
                ViewData["listTargetAdmPages"] = this.dualListAdmPage.Target;

                this.dualListAdmUser = await loadAdmUsers(admProfile, true);
                ViewData["listSourceAdmUsers"] = this.dualListAdmUser.Source;
                ViewData["listTargetAdmUsers"] = this.dualListAdmUser.Target;

                return View(admProfile);
            } else
            {
                AdmProfile admProfile = new AdmProfile();

                this.dualListAdmPage = await loadAdmPages(admProfile, false);
                ViewData["listSourceAdmPages"] = this.dualListAdmPage.Source;
                ViewData["listTargetAdmPages"] = this.dualListAdmPage.Target;

                this.dualListAdmUser = await loadAdmUsers(admProfile, false);
                ViewData["listSourceAdmUsers"] = this.dualListAdmUser.Source;
                ViewData["listTargetAdmUsers"] = this.dualListAdmUser.Target;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Administrator,Description,General,AdmPages,AdmUsers")] AdmProfile admProfile)
        {
            if (id > 0)
            {
                if (id != admProfile.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _service.Update(id, admProfile);
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
                    await _service.Insert(admProfile);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admProfile);
        }

        // DELETE: AdmProfile/Delete/5
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
