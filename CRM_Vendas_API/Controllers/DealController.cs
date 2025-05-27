using AutoMapper;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Entities.DTOs.DealDto;
using CRM_Vendas_API.Entities.DTOs.InteractionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CRM_Vendas_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DealController : ControllerBase
    {
        private readonly IDealRepository _dealRepository;
        private readonly ILogger<DealController> _logger;
        private readonly IMapper _mapper;

        public DealController(IDealRepository dealRepository, ILogger<DealController> logger, IMapper mapper)
        {
            _dealRepository = dealRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Deal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os negócios.");
            var deals = await _dealRepository.GetAllAsync();

            var dtos = deals.Select(d => new DealDto
            {
                Id = d.Id,
                Title = d.Title,
                Value = d.Value,
                Stage = d.Stage,
                CreatedAt = d.CreatedAt,
                CustomerId = d.CustomerId,
                Customer = d.Customer,
                LeadId = d.LeadId,
                Lead = d.Lead,
            });

            return Ok(dtos);
        }

        // GET: api/Deal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DealDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando negócio com id {DealId}", id);

            var deal = await _dealRepository.GetByIdAsync(id);

            if (deal == null)
            {
                _logger.LogWarning("Negócio com id {DealId} não encontrado.", id);
                return NotFound();
            }

            return Ok(_mapper.Map<DealDto>(deal));
        }

        // POST: api/Deal
        [HttpPost]
        public async Task<ActionResult<DealDto>> Create(DealCreateDto dto)
        {
            _logger.LogInformation("Criando novo negócio.");

            var deal = _mapper.Map<Deal>(dto);
            deal.CreatedAt = DateTime.UtcNow;

            await _dealRepository.AddAsync(deal);

            var dealDto = _mapper.Map<DealDto>(deal);

            return CreatedAtAction(nameof(GetById), new { id = deal.Id }, dealDto);
        }

        // PUT: api/Deal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DealUpdateDto dto)
        {
            _logger.LogInformation("Atualizando negócio com id {DealId}", id);

            var deal = await _dealRepository.GetByIdAsync(id);
            if (deal == null)
            {
                _logger.LogWarning("Negócio com id {DealId} não encontrado.", id);
                return NotFound();
            }

            _mapper.Map(dto, deal);
            await _dealRepository.UpdateAsync(deal);

            return NoContent();
        }

        // DELETE: api/Deal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Removendo negócio com id {DealId}", id);

            var deal = await _dealRepository.GetByIdAsync(id);
            if (deal == null)
            {
                _logger.LogWarning("Negócio com id {DealId} não encontrado.", id);
                return NotFound();
            }

            await _dealRepository.DeleteAsync(deal);

            return NoContent();
        }
    }
}
