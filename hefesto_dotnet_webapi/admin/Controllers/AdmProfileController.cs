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
        private readonly dbhefestoContext _context;
        private readonly IAdmProfileService _service;

        public AdmProfileController(dbhefestoContext context, IAdmProfileService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> listPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            _service.SetTransient(pagedData.page);
            return Ok(pagedData);
        }

        // GET: api/AdmProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> GetAdmProfiles()
        {
            var listAdmProfile = await _context.AdmProfiles.ToListAsync();
            _service.SetTransient(listAdmProfile);
            return listAdmProfile;
        }

        // GET: api/AdmProfile/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmProfile>> GetAdmProfile(long id)
        {
            var admProfile = await _context.AdmProfiles.FindAsync(id);

            if (admProfile == null)
            {
                return NotFound();
            } else {
                _service.SetTransient(admProfile);
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

            _context.Entry(admProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AdmProfile
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmProfile>> PostAdmProfile(AdmProfile admProfile)
        {
            _context.AdmProfiles.Add(admProfile);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdmProfileExists(admProfile.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmProfile", new { id = admProfile.Id }, admProfile);
        }

        // DELETE: api/AdmProfile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmProfile(long id)
        {
            var admProfile = await _context.AdmProfiles.FindAsync(id);
            if (admProfile == null)
            {
                return NotFound();
            }

            _context.AdmProfiles.Remove(admProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmProfileExists(long id)
        {
            return _context.AdmProfiles.Any(e => e.Id == id);
        }

        [HttpGet("mountMenu")]
        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> mountMenu(List<long> listaIdProfile){
            return await _service.mountMenuItem(listaIdProfile);
        }
        
        [HttpGet("findProfilesByPage/{pageId}")]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> findProfilesByPage(long pageId) {		
            return await _service.findProfilesByPage(pageId);
        }
        
        [HttpGet("findProfilesByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<AdmProfile>>> findProfilesByUser(long userId) {		
            return await _service.findProfilesByUser(userId);
    	}

    }
}
