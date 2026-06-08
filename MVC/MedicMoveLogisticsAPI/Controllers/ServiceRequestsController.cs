using MedicMoveLogisticsAPI.Data;
using MedicMoveLogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicMoveLogisticsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ServiceRequestsController(ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: api/ServiceRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetAll()
        {
            //load contracts
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(_context.ServiceRequests.Include(r => r.Contract));

        }

        // GET: api/ServiceRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetById(int id)
        {
            var serviceRequest = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ServiceRequest>(_context.ServiceRequests.Include(r => r.Contract));
        

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return serviceRequest;
        }

        // POST: api/ServiceRequests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> Create(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contract.FindAsync(serviceRequest.ContractId);
            if (contract == null || contract.Status != "Active")
            {
                return BadRequest("The selected contract is not active");
            }

            decimal exchangeRate = 0;
            try
            {
                var strategy = CurrencyFactory.Create(serviceRequest.CurrencyFrom, _httpClientFactory, _configuration);
                exchangeRate = await strategy.GetRateAsync();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            // compute final cost and save
            serviceRequest.Cost = serviceRequest.Amount * exchangeRate;

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = serviceRequest.Id }, serviceRequest);
        }





        //PUT: api/ServiceRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return BadRequest();

            }
            _context.Entry(serviceRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceRequestExists(id)) {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }


        // DELETE: api/ServiceRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
                return NotFound();


            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }
  


private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.Id == id);
        }
    }

}