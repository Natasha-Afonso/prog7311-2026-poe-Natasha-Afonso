using MedicMoveLogisticsAPI.Data;
using MedicMoveLogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//Emeris School of Computer Science, 2025
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
            var serviceRequest = await _context.ServiceRequests
            .Include(r => r.Contract).FirstOrDefaultAsync(r => r.Id == id);
        

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
                return BadRequest("ID mismatch");

            }

            var existingRequest = await _context.ServiceRequests.FindAsync(id);
            if (existingRequest == null)
            {
                return NotFound();
            }

         //mapping the updated fieilds over the DB
         existingRequest.Status = serviceRequest.Status;
         existingRequest.Amount = serviceRequest.Amount;
         existingRequest.Cost = serviceRequest.Cost;
         existingRequest.CurrencyFrom = serviceRequest.CurrencyFrom;
         existingRequest.Description = serviceRequest.Description;
         existingRequest.ContractId = serviceRequest.ContractId;

         
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
/** 
References
Chandra, S. 2026. JWT Authentication in ASP.NET Core: A complete step-bystey guide. (Version 10.0) [Source Code]. Available at: <https://medium.com/@Rajdip27/jwt-authentication-in-asp-net-core-a-complete-step-by-step-guide-94c955d907da> [Accessed 7 June 2026]
CodeWithGopi. 2025. Create and Consume Web API in ASP.NET Core MVC | Full CRUD Operatins & API Consumption. (Version 10.0) [Source Code]. Available at:<https://youtu.be/knTcwvFWOQM?si=b3q3ksLg6HNnBjDg> [Accessed 4 June 2026]
Emad, C. 2024. ASP.NET Core Web API with JWT (JSON Web Token) authentication. (Version 10.0) [Source Code]. Available at: <https://github.com/carolemad965/ASP.NET-Core-Web-API-with-JWT-Authentication> [Accessed 7 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 1 - Getting started. (Version 10.0) [Source Code]. Available at: <https://youtu.be/ldfC-rTzHPY?si=znsmPz-ZSPaY-4Ii> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 2- connecting between the two!. (Version 10.0) [Source Code]. Available at: <https://youtu.be/_0EhC3WauAc?si=nEazkCpBTV1YpHBp> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 3- databases in docker. (Version 10.0) [Source Code]. Available at: <https://youtu.be/6tYEbngWY-E?si=10rSEEpfSNL5Kg8k> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 4- connecting to database from the API.(Version 10.0) [Source Code]. Available at: <https://youtu.be/_PwpPMoeqzw?si=Q0ELlPZdN42dFU9q> [Accessed 5 June 2026]
Emeris School of Computer Science. 2025. PROG7311 Docker part 5- connecting them all together- DB -API -MVC. (Version 10.0) [Source Code]. Available at: <https://youtu.be/YlIajRMdLPk?si=D75XZKO-L6nQhR-3> [Accessed 5 June 2026]
Microsoft Learn. 2026. Parameter Binding in Minimal API apps. (Version 10)[source code]. Available at: <https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-10.0> [Accessed 5 June 2026]
Microsoft Learn. 2023. Use IHttpClientFactory to implement resilient HTTP requests. (Version 10)[source code]. Available at: <https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests> [Accessed 5 June 2026]
Verma, S. 2024. Model Binding in ASP.NET Core. (Version 10)[source code]. Available at: <https://www.c-sharpcorner.com/article/model-binding-in-asp-net-core2/> [Accessed 5 June 2026]           
Saha, D.2024. Implement Cascading Dropdown In ASP.NET Core MVC Application Using InMemory DB.(Version 10)[source code]. Available at: < https://www.c-sharpcorner.com/article/implement-cascading-dropdown-in-asp-net-core-mvc-application-using-inmemory-db/ > [Accessed 7 June 2026]
Williams, R.2026.SEND IT QUICK COURIER SERVICE.(Version 10)[source code]. Available at: < https://codepen.io/dasia16/pen/azvEBBp > [Accessed 5 June 2026]
*/
