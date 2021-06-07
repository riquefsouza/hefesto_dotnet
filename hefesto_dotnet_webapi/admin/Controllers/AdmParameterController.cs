using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAdmParameterService _service;

        public AdmParameterController(IAdmParameterService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<BasePaged<AdmParameter>>> ListPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);           
            return Ok(pagedData);
        }

        // GET: api/AdmParameter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmParameter>>> GetAdmParameters()
        {
            return await _service.FindAll();
        }

        // GET: api/AdmParameter/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmParameter>> GetAdmParameter(long id)
        {
            var admParameter = await _service.FindById(id);

            if (admParameter == null)
            {
                return NotFound();
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

            var updated = await _service.Update(id, admParameter);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AdmParameter
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmParameter>> PostAdmParameter(AdmParameter admParameter)
        {
            var created = await _service.Insert(admParameter);

            if (created == null)
            {
                return Conflict();
            }

            return CreatedAtAction("GetAdmParameter", new { id = admParameter.Id }, admParameter);
        }

        // DELETE: api/AdmParameter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmParameter(long id)
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
