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
using hefesto.base_hefesto.Pagination;

namespace hefesto_dotnet_webapi.admin.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AdmPageController : ControllerBase
    {
        private readonly dbhefestoContext _context;
        private readonly IAdmPageService _service;

        public AdmPageController(dbhefestoContext context, IAdmPageService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<AdmPage>>> listPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            _service.SetTransient(pagedData.page);
            return Ok(pagedData);
        }

        // GET: api/AdmPage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmPage>>> GetAdmPages()
        {
            var listAdmPage = await _context.AdmPages.ToListAsync();
            _service.SetTransient(listAdmPage);
            return listAdmPage;
        }

        // GET: api/AdmPage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmPage>> GetAdmPage(long id)
        {
            var admPage = await _context.AdmPages.FindAsync(id);

            if (admPage == null)
            {
                return NotFound();
            } else {
                _service.SetTransient(admPage);
            }

            return admPage;
        }

        // PUT: api/AdmPage/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmPage(long id, AdmPage admPage)
        {
            if (id != admPage.Id)
            {
                return BadRequest();
            }

            _context.Entry(admPage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmPageExists(id))
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

        // POST: api/AdmPage
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmPage>> PostAdmPage(AdmPage admPage)
        {
            _context.AdmPages.Add(admPage);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdmPageExists(admPage.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmPage", new { id = admPage.Id }, admPage);
        }

        // DELETE: api/AdmPage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmPage(long id)
        {
            var admPage = await _context.AdmPages.FindAsync(id);
            if (admPage == null)
            {
                return NotFound();
            }

            _context.AdmPages.Remove(admPage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmPageExists(long id)
        {
            return _context.AdmPages.Any(e => e.Id == id);
        }
    }
}
