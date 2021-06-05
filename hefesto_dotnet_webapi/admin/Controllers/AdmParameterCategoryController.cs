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
            return await _service.FindAll();
        }

        // GET: api/AdmParameterCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmParameterCategory>> GetAdmParameterCategory(long id)
        {
            var admParameterCategory = await _service.FindById(id);

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

            var updated = await _service.Update(id, admParameterCategory);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AdmParameterCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmParameterCategory>> PostAdmParameterCategory(AdmParameterCategory admParameterCategory)
        {
            var created = await _service.Insert(admParameterCategory);

            if (created == null)
            {
                return Conflict();
            }

            return CreatedAtAction("GetAdmParameterCategory", new { id = admParameterCategory.Id }, admParameterCategory);
        }

        // DELETE: api/AdmParameterCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmParameterCategory(long id)
        {
            var deleted = await _service.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
