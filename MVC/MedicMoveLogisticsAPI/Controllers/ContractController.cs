using MedicMoveLogisticsAPI.Data;
using MedicMoveLogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicMoveLogisticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Contract
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetAll()
        {
            return await _context.Contract.Include(c => c.Client).ToListAsync();
        }

        // GET: api/Contract/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetById(int id)
        {
            var contract = await _context.Contract.Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            
                return NotFound();
            

            return contract;
        }

        // POST: api/Contract
        [HttpPost]
        public async Task<ActionResult<Contract>> Create(Contract contract)
        {
            _context.Contract.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contract);
        }
        //PUT: api/Contract/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Contract contract)
        {
            if (id != contract.Id)
            
                return BadRequest();
            

            _context.Entry(contract).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Contract.Any(e => e.Id == id))
                
                    return NotFound();
                
                
                
                    throw;
                }
          

            return NoContent();
        }

        // DELETE: api/Contract/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
                return NotFound();
            

            _context.Contract.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}