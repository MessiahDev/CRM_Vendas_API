using AutoMapper;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Entities.DTOs.CustomerDto;
using CRM_Vendas_API.Entities.DTOs.LeadDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Vendas_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, ILogger<CustomerController> logger, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os clientes.");
            var customers = await _customerRepository.GetAllAsync();

            var dtos = customers.Select(l => new CustomerDto
            {
                Id = l.Id,
                Name = l.Name,
                Email = l.Email,
                Phone = l.Phone,
                ConvertedAt = l.ConvertedAt,
                UserId = l.UserId,
                User = l.User
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando cliente com id {CustomerId}", id);
            var customer = await _customerRepository.GetByIdAsync(id);
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

            await _customerRepository.AddAsync(customer);

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customerDto);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CustomerUpdateDto dto)
        {
            _logger.LogInformation("Atualizando cliente com id {CustomerId}", id);

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Cliente com id {CustomerId} não encontrado.", id);
                return NotFound();
            }

            _mapper.Map(dto, customer);
            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Excluindo cliente com id {CustomerId}", id);

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Cliente com id {CustomerId} não encontrado.", id);
                return NotFound();
            }

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}
