using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;
using hefesto.admin.Services;
using Microsoft.AspNetCore.Authorization;
using hefesto.base_hefesto.Models;
using hefesto.base_hefesto.Pagination;

namespace hefesto_dotnet_webapi.admin.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AdmProfileController : ControllerBase
    {
        private readonly IAdmProfileService _service;

        public AdmProfileController(IAdmProfileService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<BasePaged<AdmProfile>>> ListPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            return Ok(pagedData);
        }

        // GET: api/AdmProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> GetAdmProfiles()
        {
            return await _service.FindAll();
        }

        // GET: api/AdmProfile/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmProfile>> GetAdmProfile(long id)
        {
            var admProfile = await _service.FindById(id);

            if (admProfile == null)
            {
                return NotFound();
            }

            return admProfile;
        }

        // PUT: api/AdmProfile/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmProfile(long id, AdmProfile admProfile)
        {
            if (id != admProfile.Id)
            {
                return BadRequest();
            }

            var updated = await _service.Update(id, admProfile);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AdmProfile
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmProfile>> PostAdmProfile(AdmProfile admProfile)
        {
            var created = await _service.Insert(admProfile);

            if (created == null)
            {
                return Conflict();
            }

            return CreatedAtAction("GetAdmProfile", new { id = admProfile.Id }, admProfile);
        }

        // DELETE: api/AdmProfile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmProfile(long id)
        {
            var deleted = await _service.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("mountMenu")]
        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> mountMenu(List<long> listaIdProfile){
            return await _service.MountMenuItem(listaIdProfile);
        }
        
        [HttpGet("findProfilesByPage/{pageId}")]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> findProfilesByPage(long pageId) {		
            return await _service.FindProfilesByPage(pageId);
        }
        
        [HttpGet("findProfilesByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> findProfilesByUser(long userId) {		
            return await _service.FindProfilesByUser(userId);
    	}

    }
}
