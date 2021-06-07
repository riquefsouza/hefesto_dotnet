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
        private readonly IAdmPageService _service;

        public AdmPageController(IAdmPageService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<BasePaged<AdmPage>>> ListPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            return Ok(pagedData);
        }

        // GET: api/AdmPage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmPage>>> GetAdmPages()
        {
            return await _service.FindAll();
        }

        // GET: api/AdmPage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmPage>> GetAdmPage(long id)
        {
            var admPage = await _service.FindById(id);

            if (admPage == null)
            {
                return NotFound();
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

            var updated = await _service.Update(id, admPage);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AdmPage
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmPage>> PostAdmPage(AdmPage admPage)
        {
            var created = await _service.Insert(admPage);

            if (created == null)
            {
                return Conflict();
            }

            return CreatedAtAction("GetAdmPage", new { id = admPage.Id }, admPage);
        }

        // DELETE: api/AdmPage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmPage(long id)
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
