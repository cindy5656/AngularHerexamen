using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly DataContext _context;

        public CompanyController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var Company = await _context.Companies.FindAsync(id);

            if (Company == null)
            {
                return NotFound();
            }

            return Company;
        }

        // GET: api/Company/5
        [HttpGet("User/{userID}")]
        public async Task<ActionResult<Company>> GetCompanyByUserID(int userID)
        {
            var Company = await _context.Companies.Where(x => x.CompanyManagerID == userID).FirstOrDefaultAsync();

            if (Company == null)
            {
                return NotFound();
            }

            return Company;
        }

        [HttpGet("Beheerder/{userID}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroepenByBeheerder(int userID)
        {
            var groepen = await _context.CompanyUserGroup.Where(x => x.role.Name == "Beheerder" && x.group != null).Select(x => x.group).Distinct().ToListAsync();

            if (groepen == null)
            {
                return NotFound();
            }

            return groepen;
        }

        [HttpGet("Werknemers/{companyID}")]
        public async Task<ActionResult<IEnumerable<User>>> GetWerknemers(int companyID)
        {
            var users = await _context.CompanyUserGroup.Where(x => x.CompanyID == companyID && x.role.Name == "Werknemer" && x.user != null).Select(x => x.user).Distinct().ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        [HttpGet("Groepsleden")]
        public async Task<ActionResult<IEnumerable<User>>> GetLeden(int groupID, int companyID)
        {
            var users = await _context.CompanyUserGroup.Where(x => x.CompanyID == companyID && x.role.Name == "Groepslid" && x.GroupID == groupID && x.user != null).Select(x => x.user).Distinct().ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        [HttpGet("GetBeheerdersRechtenFromUserID")]
        public async Task<ActionResult<IEnumerable<Role>>> GetBeheerdersRechtenFromUserID(int companyID, int userID)
        {
            var roles = await _context.CompanyUserGroup.Where(x => x.CompanyID == companyID  && x.UserID == userID).Select(x => x.role).Distinct().ToListAsync();

            if (roles == null)
            {
                return NotFound();
            }

            return roles;
        }

        [HttpGet("Groepen/{companyID}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroepen(int companyID)
        {
            var groepen = await _context.CompanyUserGroup.Where(x => x.CompanyID == companyID && x.group != null).Select(x => x.group).Distinct().ToListAsync();

            if (groepen == null)
            {
                return NotFound();
            }

            return groepen;
        }

        [HttpGet("GetCompanyFromGroup/{groupID}")]
        public async Task<ActionResult<Company>> GetCompanyFromGroup(int groupID)
        {
            var company = await _context.CompanyUserGroup.Where(x => x.GroupID == groupID && x.company != null).Select(x => x.company).Distinct().FirstOrDefaultAsync();

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Company/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company Company)
        {
            if (id != Company.CompanyID)
            {
                return BadRequest();
            }

            _context.Entry(Company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Company
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company Company)
        {
            _context.Companies.Add(Company);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyExists(Company.CompanyID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCompany", new { id = Company.CompanyID }, Company);
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var Company = await _context.Companies.FindAsync(id);
            if (Company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(Company);
            await _context.SaveChangesAsync();

            return Company;
        }

        [HttpDelete("RolWegnemen")]
        public async Task<ActionResult<CompanyUserGroup>> RolWegnemen(int companyID, int userID, int roleID, int? groupID)
        {
            var CompanyGroupUser = await _context.CompanyUserGroup.Where(x => x.CompanyID == companyID && x.RoleID== roleID && x.UserID == userID && (groupID == null || groupID == 0 || x.GroupID == groupID)).FirstOrDefaultAsync();
            if (CompanyGroupUser == null)
            {
                return NotFound();
            }

            _context.CompanyUserGroup.Remove(CompanyGroupUser);
            await _context.SaveChangesAsync();

            return CompanyGroupUser;
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyID == id);
        }

        [HttpPost("User")]
        public async Task<ActionResult<Company>> AddUserToCompany(int companyID, int userID, int roleID, int? groupID)
        {
            CompanyUserGroup companyUserGroup = new CompanyUserGroup();
            companyUserGroup.CompanyID = companyID;
            companyUserGroup.company = _context.Companies.Where(x => x.CompanyID == companyID).FirstOrDefault();
            companyUserGroup.UserID = userID;
            companyUserGroup.RoleID = roleID;
            companyUserGroup.GroupID = groupID;
            _context.CompanyUserGroup.Add(companyUserGroup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyExists(companyUserGroup.CompanyGroupUserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCompany", new { id = companyUserGroup.CompanyID }, companyUserGroup.company);
        }
    }
}
