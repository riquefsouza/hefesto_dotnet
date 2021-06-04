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
    public class AdmMenuController : ControllerBase
    {
        private readonly dbhefestoContext _context;

        private readonly IAdmMenuService _service;

        public AdmMenuController(dbhefestoContext context, IAdmMenuService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<AdmMenu>>> listPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            _service.SetTransient(pagedData.page);
            return Ok(pagedData);
        }

        // GET: api/AdmMenu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmMenu>>> GetAdmMenus()
        {
            var listAdmMenu = await _context.AdmMenus.ToListAsync();
            _service.SetTransient(listAdmMenu);
            return listAdmMenu;
        }

        // GET: api/AdmMenu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmMenu>> GetAdmMenu(long id)
        {
            var admMenu = await _context.AdmMenus.FindAsync(id);

            if (admMenu == null)
            {
                return NotFound();
            } else {
                _service.SetTransient(admMenu);
            }

            return admMenu;
        }

        // PUT: api/AdmMenu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmMenu(long id, AdmMenu admMenu)
        {
            if (id != admMenu.Id)
            {
                return BadRequest();
            }

            _context.Entry(admMenu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmMenuExists(id))
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

        // POST: api/AdmMenu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmMenu>> PostAdmMenu(AdmMenu admMenu)
        {
            _context.AdmMenus.Add(admMenu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdmMenuExists(admMenu.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmMenu", new { id = admMenu.Id }, admMenu);
        }

        // DELETE: api/AdmMenu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmMenu(long id)
        {
            var admMenu = await _context.AdmMenus.FindAsync(id);
            if (admMenu == null)
            {
                return NotFound();
            }

            _context.AdmMenus.Remove(admMenu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmMenuExists(long id)
        {
            return _context.AdmMenus.Any(e => e.Id == id);
        }
    }
}
