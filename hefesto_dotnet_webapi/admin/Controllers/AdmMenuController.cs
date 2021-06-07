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
        private readonly IAdmMenuService _service;

        public AdmMenuController(IAdmMenuService service)
        {
            _service = service;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<BasePaged<AdmMenu>>> ListPaged([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var pagedData = await _service.GetPage(route, filter);
            return Ok(pagedData);
        }

        // GET: api/AdmMenu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmMenu>>> GetAdmMenus()
        {
            return await _service.FindAll();
        }

        // GET: api/AdmMenu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmMenu>> GetAdmMenu(long id)
        {
            var admMenu = await _service.FindById(id);

            if (admMenu == null)
            {
                return NotFound();
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

            var updated = await _service.Update(id, admMenu);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/AdmMenu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdmMenu>> PostAdmMenu(AdmMenu admMenu)
        {
            var created = await _service.Insert(admMenu);

            if (created == null)
            {
                return Conflict();
            }

            return CreatedAtAction("GetAdmMenu", new { id = admMenu.Id }, admMenu);
        }

        // DELETE: api/AdmMenu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmMenu(long id)
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
