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
    public class AdmParameterController : ControllerBase
    {
        private readonly dbhefestoContext _context;

        private readonly IAdmParameterService _service;

        public AdmParameterController(dbhefestoContext context, IAdmParameterService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<AdmParameter>>> listPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            _service.SetTransient(pagedData.page);
            return Ok(pagedData);
        }

        // GET: api/AdmParameter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmParameter>>> GetAdmParameters()
        {
            var listAdmParameter = await _context.AdmParameters.ToListAsync();
            _service.SetTransient(listAdmParameter);
            return listAdmParameter;
        }

        // GET: api/AdmParameter/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmParameter>> GetAdmParameter(long id)
        {
            var admParameter = await _context.AdmParameters.FindAsync(id);

            if (admParameter == null)
            {
                return NotFound();
            } else {
                _service.SetTransient(admParameter);
            }

            return admParameter;
        }

        // PUT: api/AdmParameter/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmParameter(long id, AdmParameter admParameter)
        {
            if (id != admParameter.Id)
            {
                return BadRequest();
            }

            _context.Entry(admParameter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmParameterExists(id))
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

        // POST: api/AdmParameter
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmParameter>> PostAdmParameter(AdmParameter admParameter)
        {
            _context.AdmParameters.Add(admParameter);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdmParameterExists(admParameter.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmParameter", new { id = admParameter.Id }, admParameter);
        }

        // DELETE: api/AdmParameter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmParameter(long id)
        {
            var admParameter = await _context.AdmParameters.FindAsync(id);
            if (admParameter == null)
            {
                return NotFound();
            }

            _context.AdmParameters.Remove(admParameter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmParameterExists(long id)
        {
            return _context.AdmParameters.Any(e => e.Id == id);
        }
    }
}
