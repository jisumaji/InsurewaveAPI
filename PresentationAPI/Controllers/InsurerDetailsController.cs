using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;

namespace PresentationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurerDetailsController : ControllerBase
    {
        private readonly InsurewaveContext _context;

        public InsurerDetailsController(InsurewaveContext context)
        {
            _context = context;
        }

        // GET: api/InsurerDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsurerDetail>>> GetInsurerDetails()
        {
          if (_context.InsurerDetails == null)
          {
              return NotFound();
          }
            return await _context.InsurerDetails.ToListAsync();
        }

        // GET: api/InsurerDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InsurerDetail>> GetInsurerDetail(string id)
        {
          if (_context.InsurerDetails == null)
          {
              return NotFound();
          }
            var insurerDetail = await _context.InsurerDetails.FindAsync(id);

            if (insurerDetail == null)
            {
                return NotFound();
            }

            return insurerDetail;
        }

        // PUT: api/InsurerDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurerDetail(string id, InsurerDetail insurerDetail)
        {
            if (id != insurerDetail.InsurerId)
            {
                return BadRequest();
            }

            _context.Entry(insurerDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsurerDetailExists(id))
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

        // POST: api/InsurerDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InsurerDetail>> PostInsurerDetail(InsurerDetail insurerDetail)
        {
          if (_context.InsurerDetails == null)
          {
              return Problem("Entity set 'InsurewaveContext.InsurerDetails'  is null.");
          }
            _context.InsurerDetails.Add(insurerDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InsurerDetailExists(insurerDetail.InsurerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetInsurerDetail", new { id = insurerDetail.InsurerId }, insurerDetail);
        }

        // DELETE: api/InsurerDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurerDetail(string id)
        {
            if (_context.InsurerDetails == null)
            {
                return NotFound();
            }
            var insurerDetail = await _context.InsurerDetails.FindAsync(id);
            if (insurerDetail == null)
            {
                return NotFound();
            }

            _context.InsurerDetails.Remove(insurerDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsurerDetailExists(string id)
        {
            return (_context.InsurerDetails?.Any(e => e.InsurerId == id)).GetValueOrDefault();
        }
    }
}
