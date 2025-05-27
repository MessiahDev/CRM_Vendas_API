using AutoMapper;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Entities.DTOs.DealDto;
using CRM_Vendas_API.Entities.DTOs.LeadDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Vendas_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILogger<LeadController> _logger;
        private readonly IMapper _mapper;

        public LeadController(ILeadRepository leadRepository, ILogger<LeadController> logger, IMapper mapper)
        {
            _leadRepository = leadRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Lead
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os leads.");
            var leads = await _leadRepository.GetAllAsync();

            var dtos = leads.Select(l => new LeadDto
            {
                Id = l.Id,
                Name = l.Name,
                Email = l.Email,
                Phone = l.Phone,
                Source = l.Source,
                Status = l.Status,
                CreatedAt = l.CreatedAt,
                UserId = l.UserId,
                UserName = l.User?.Name,
                CustomerId = l.CustomerId,
                CustomerName = l.Customer?.Name,
            });

            return Ok(dtos);
        }

        // GET: api/Lead/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeadDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando lead com id {LeadId}", id);
            var lead = await _leadRepository.GetByIdAsync(id);

            if (lead == null)
            {
                _logger.LogWarning("Lead com id {LeadId} não encontrado.", id);
                return NotFound();
            }

            return Ok(_mapper.Map<LeadDto>(lead));
        }

        // POST: api/Lead
        [HttpPost]
        public async Task<ActionResult<LeadDto>> CreateLead([FromBody] LeadCreateDto dto)
        {
            _logger.LogInformation("Criando novo lead.");

            var lead = _mapper.Map<Lead>(dto);
            lead.CreatedAt = DateTime.UtcNow;

            await _leadRepository.AddAsync(lead);

            var leadDto = _mapper.Map<LeadDto>(lead);

            return CreatedAtAction(nameof(GetById), new { id = lead.Id }, leadDto);
        }

        // PUT: api/Lead/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LeadUpdateDto dto)
        {
            _logger.LogInformation("Atualizando lead com id {LeadId}", id);

            var lead = await _leadRepository.GetByIdAsync(id);
            if (lead == null)
            {
                _logger.LogWarning("Lead com id {LeadId} não encontrado.", id);
                return NotFound();
            }

            _mapper.Map(dto, lead);
            await _leadRepository.UpdateAsync(lead);

            return NoContent();
        }

        // DELETE: api/Lead/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Excluindo lead com id {LeadId}", id);

            var lead = await _leadRepository.GetByIdAsync(id);
            if (lead == null)
            {
                _logger.LogWarning("Lead com id {LeadId} não encontrado.", id);
                return NotFound();
            }

            await _leadRepository.DeleteAsync(lead);

            return NoContent();
        }
    }
}
