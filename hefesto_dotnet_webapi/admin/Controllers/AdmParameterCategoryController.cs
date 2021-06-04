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
    public class AdmParameterCategoryController : ControllerBase
    {
        private readonly dbhefestoContext _context;

        private readonly IAdmParameterCategoryService _service;

        public AdmParameterCategoryController(dbhefestoContext context, IAdmParameterCategoryService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<AdmParameterCategory>>> listPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            return Ok(pagedData);
        }

        // GET: api/AdmParameterCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmParameterCategory>>> GetAdmParameterCategories()
        {
            return await _context.AdmParameterCategories.ToListAsync();
        }

        // GET: api/AdmParameterCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmParameterCategory>> GetAdmParameterCategory(long id)
        {
            var admParameterCategory = await _context.AdmParameterCategories.FindAsync(id);

            if (admParameterCategory == null)
            {
                return NotFound();
            }

            return admParameterCategory;
        }

        // PUT: api/AdmParameterCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmParameterCategory(long id, AdmParameterCategory admParameterCategory)
        {
            if (id != admParameterCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(admParameterCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmParameterCategoryExists(id))
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

        // POST: api/AdmParameterCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmParameterCategory>> PostAdmParameterCategory(AdmParameterCategory admParameterCategory)
        {
            _context.AdmParameterCategories.Add(admParameterCategory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdmParameterCategoryExists(admParameterCategory.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdmParameterCategory", new { id = admParameterCategory.Id }, admParameterCategory);
        }

        // DELETE: api/AdmParameterCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmParameterCategory(long id)
        {
            var admParameterCategory = await _context.AdmParameterCategories.FindAsync(id);
            if (admParameterCategory == null)
            {
                return NotFound();
            }

            _context.AdmParameterCategories.Remove(admParameterCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmParameterCategoryExists(long id)
        {
            return _context.AdmParameterCategories.Any(e => e.Id == id);
        }
    }
}
