using AutoMapper;
using CRM_Vendas_API.Context;
using CRM_Vendas_API.Entities.DTOs.CustomerDto;
using CRM_Vendas_API.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;

        public CustomerController(AppDbContext context, ILogger<CustomerController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os clientes.");
            var customers = await _context.Customers.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando cliente com id {CustomerId}", id);
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Cliente com id {CustomerId} não encontrado.", id);
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CustomerCreateDto dto)
        {
            _logger.LogInformation("Criando novo cliente.");

            var customer = _mapper.Map<Customer>(dto);
            customer.ConvertedAt = DateTime.UtcNow;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customerDto);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CustomerUpdateDto dto)
        {
            _logger.LogInformation("Atualizando cliente com id {CustomerId}", id);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Cliente com id {CustomerId} não encontrado.", id);
                return NotFound();
            }

            _mapper.Map(dto, customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Excluindo cliente com id {CustomerId}", id);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Cliente com id {CustomerId} não encontrado.", id);
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
