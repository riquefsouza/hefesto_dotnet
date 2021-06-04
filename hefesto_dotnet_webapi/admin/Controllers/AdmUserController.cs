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
using Microsoft.Extensions.Logging;
using hefesto.base_hefesto.Pagination;

namespace hefesto_dotnet_webapi.admin.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AdmUserController : ControllerBase
    {
        private readonly ILogger<AdmUserController> _logger;
        private readonly dbhefestoContext _context;
        private readonly IAdmUserService _service;

        public AdmUserController(dbhefestoContext context, IAdmUserService service,
            ILogger<AdmUserController> logger)
        {
            _context = context;
            _service = service;
            _logger = logger;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<AdmUser>>> listPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            _service.SetTransient(pagedData.page);
            return Ok(pagedData);
        }

        // GET: api/AdmUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmUser>>> GetAdmUsers()
        {
            var listAdmUser = await _context.AdmUsers.ToListAsync();
            _service.SetTransient(listAdmUser);
            return listAdmUser;
        }

        // GET: api/AdmUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmUser>> GetAdmUser(long id)
        {
            var admUser = await _context.AdmUsers.FindAsync(id);

            if (admUser == null)
            {
                return NotFound();
            } else {
                _service.SetTransient(admUser);
            }

            return admUser;
        }

        // PUT: api/AdmUser/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmUser(long id, AdmUser admUser)
        {
            if (id != admUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(admUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmUserExists(id))
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

        // POST: api/AdmUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmUser>> PostAdmUser(AdmUser admUser)
        {
            _context.AdmUsers.Add(admUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdmUserExists(admUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmUser", new { id = admUser.Id }, admUser);
        }

        // DELETE: api/AdmUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmUser(long id)
        {
            var admUser = await _context.AdmUsers.FindAsync(id);
            if (admUser == null)
            {
                return NotFound();
            }

            _context.AdmUsers.Remove(admUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmUserExists(long id)
        {
            return _context.AdmUsers.Any(e => e.Id == id);
        }
    }
}
