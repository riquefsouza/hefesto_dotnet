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
        private readonly IAdmUserService _service;

        public AdmUserController(IAdmUserService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<BasePaged<AdmUser>>> ListPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            return Ok(pagedData);
        }

        // GET: api/AdmUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmUser>>> GetAdmUsers()
        {
            return await _service.FindAll();
        }

        // GET: api/AdmUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmUser>> GetAdmUser(long id)
        {
            var admUser = await _service.FindById(id);

            if (admUser == null)
            {
                return NotFound();
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

            var updated = await _service.Update(id, admUser);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AdmUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmUser>> PostAdmUser(AdmUser admUser)
        {
            var created = await _service.Insert(admUser);

            if (created == null)
            {
                return Conflict();
            }

            return CreatedAtAction("GetAdmUser", new { id = admUser.Id }, admUser);
        }

        // DELETE: api/AdmUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmUser(long id)
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
