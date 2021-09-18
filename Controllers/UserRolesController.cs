using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PicNic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly PicNicContext _context;
        private UserManager<AppUser> _userManager;

        public UserRolesController(PicNicContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUserRoles>>> GetAspNetUserRoles()
        {
            return await _context.AspNetUserRoles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AspNetUserRoles>> GetAspNetUserRoles(string id)
        {
            var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);

            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            return aspNetUserRoles;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUserRoles(string id, AddUserToRoleDto dto)
        {
            if (id != dto.UserId)
            {
                return BadRequest();
            }

            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                var result = await _userManager.AddToRoleAsync(user, dto.RoleName);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserRolesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public class AddUserToRoleDto
        {
            public string UserId { get; set; }
            public string RoleName { get; set; }
        }

        // POST: api/Roles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AspNetUserRoles>> PostAspNetUserRoles(AspNetUserRoles aspNetUserRoles)
        {
            _context.AspNetUserRoles.Add(aspNetUserRoles);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserRolesExists(aspNetUserRoles.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAspNetUserRoles", new { id = aspNetUserRoles.UserId }, aspNetUserRoles);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetUserRoles>> DeleteAspNetUserRoles(string id)
        {
            var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            _context.AspNetUserRoles.Remove(aspNetUserRoles);
            await _context.SaveChangesAsync();

            return aspNetUserRoles;
        }

        private bool AspNetUserRolesExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }
    }
}