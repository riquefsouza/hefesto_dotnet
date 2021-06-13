using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Models;
using hefesto.admin.Services;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Report;
using hefesto.base_hefesto;
using Microsoft.AspNetCore.Authorization;

namespace hefesto_dotnet_mvc.admin.Controllers
{
    public class AdmPageController : BaseViewReportController
    {
        private readonly IMessageService _messageService;

        private readonly IAdmPageService _service;

        private readonly IAdmProfileService admProfileService;

        private BaseDualList<AdmProfile> dualListAdmProfile;

        private List<AdmProfile> listAllAdmProfiles;

        public AdmPageController(IAdmPageService service, IAdmProfileService admProfileService, 
            IMessageService messageService, ISystemService systemService) : base(messageService, systemService)
        {
            _service = service;            
            _messageService = messageService;
            this.admProfileService = admProfileService;

            this.listAllAdmProfiles = new List<AdmProfile>();
        }

        private async Task<BaseDualList<AdmProfile>> loadAdmProfiles(AdmPage bean, bool bEdit)
        {
            List<AdmProfile> listAdmProfilesSelected;
            List<AdmProfile> listAdmProfiles = await admProfileService.FindAll();

            listAllAdmProfiles.Clear();
            listAllAdmProfiles.AddRange(listAdmProfiles);

            if (bEdit)
            {
                listAdmProfilesSelected = new List<AdmProfile>();

                foreach (AdmProfile profile in listAdmProfiles)
                {
                    foreach (AdmPage page in profile.AdmPages)
                    {
                        if (page.Equals(bean))
                        {
                            listAdmProfilesSelected.Add(profile);
                            break;
                        }
                    }
                }

                listAdmProfiles.RemoveAll(item => listAdmProfilesSelected.Contains(item));
            }
            else
            {
                listAdmProfilesSelected = new List<AdmProfile>();
            }

            this.dualListAdmProfile = new BaseDualList<AdmProfile>(listAdmProfiles, listAdmProfilesSelected);

            return this.dualListAdmProfile;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            LoadMessages();

            var listAdmProfile = await _service.FindAll();
            return View(listAdmProfile);
        }

        // GET: AdmPage/Edit/{id}
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoadMessages();

            if (id > 0)
            {
                var admPage = await _service.FindById(id);
                if (admPage == null)
                {
                    return NotFound();
                }

                this.dualListAdmProfile = await loadAdmProfiles(admPage, true);
                ViewData["listSourceAdmProfiles"] = this.dualListAdmProfile.Source;
                ViewData["listTargetAdmProfiles"] = this.dualListAdmProfile.Target;

                return View(admPage);
            } else
            {
                AdmPage admPage = new AdmPage();

                this.dualListAdmProfile = await loadAdmProfiles(admPage, false);
                ViewData["listSourceAdmProfiles"] = this.dualListAdmProfile.Source;
                ViewData["listTargetAdmProfiles"] = this.dualListAdmProfile.Target;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(long id, 
            [Bind("Id,Description,Url,AdmIdProfiles")] AdmPage admPage)
        {
            if (id > 0)
            {
                if (id != admPage.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _service.Update(id, admPage);
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
                    await _service.Insert(admPage);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(admPage);
        }

        // DELETE: AdmPage/Delete/5
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
