using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using prjHealthCareSystem.Data;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Client.ToListAsync());
            //Returns the whole list stored in the db
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client); //Adds the new client 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
            ////Details for a specific patient
            public async Task<IActionResult>Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            //Find the client by their id
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id); //LingQ
            if (client == null)
            {
                return NotFound();
            }
            return View(client);

        }

        //Delete Methods
     public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if(client == null)
            {
                return NotFound();
            }
           
            return View(client);
            
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if(client != null)
            {
                _context.Client.Remove(client);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}