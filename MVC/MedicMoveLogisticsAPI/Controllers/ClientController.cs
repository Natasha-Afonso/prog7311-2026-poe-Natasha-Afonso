using MedicMoveLogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicMoveLogisticsAPI.Data;
using Microsoft.AspNetCore.Authorization;
//Emeris School of Computer Science, 2025
namespace MedicMoveLogisticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //secures the API, only authenticated users can access it
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            return await _context.Client.ToListAsync();
        }

        // GET: api/Client/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
                return NotFound();

            return client;
        }

        // POST: api/Client
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Client.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        // PUT: api/Client/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
                return BadRequest();
            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Client.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Client/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
                return NotFound();

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }

}
/* References
* Emeris School of Computer Science. 2025. PROG7311 Docker part 1 - Getting started. (Version 10.0) [Source Code]. Available at: <https://youtu.be/ldfC-rTzHPY?si=znsmPz-ZSPaY-4Ii> [Accessed 5 June 2026]
* Emeris School of Computer Science. 2025. PROG7311 Docker part 2- connecting between the two!. (Version 10.0) [Source Code]. Available at: <https://youtu.be/_0EhC3WauAc?si=nEazkCpBTV1YpHBp> [Accessed 5 June 2026]
* Emeris School of Computer Science. 2025. PROG7311 Docker part 3- databases in docker. (Version 10.0) [Source Code]. Available at: <https://youtu.be/6tYEbngWY-E?si=10rSEEpfSNL5Kg8k> [Accessed 5 June 2026]
* Emeris School of Computer Science. 2025. PROG7311 Docker part 4- connecting to database from the API.(Version 10.0) [Source Code]. Available at: <https://youtu.be/_PwpPMoeqzw?si=Q0ELlPZdN42dFU9q> [Accessed 5 June 2026]
* Emeris School of Computer Science. 2025. PROG7311 Docker part 5- connecting them all together- DB -API -MVC. (Version 10.0) [Source Code]. Available at: <https://youtu.be/YlIajRMdLPk?si=D75XZKO-L6nQhR-3> [Accessed 5 June 2026]*/