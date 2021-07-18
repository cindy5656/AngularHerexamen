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

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyID == id);
        }
    }
}
