using AutoMapper;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Entities.DTOs.InteractionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Vendas_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionsRepository _interactionsRepository;
        private readonly ILogger<InteractionController> _logger;
        private readonly IMapper _mapper;

        public InteractionController(IInteractionsRepository interactionsRepository, ILogger<InteractionController> logger, IMapper mapper)
        {
            _interactionsRepository = interactionsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Interaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InteractionDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todas as interações.");
            var interactions = await _interactionsRepository.GetAllAsync();

            var dtos = interactions.Select(i => new InteractionDto
            {
                Id = i.Id,
                Type = i.Type,
                Notes = i.Notes,
                Date = i.Date,
                LeadId = i.LeadId,
                Lead = i.Lead,
                CustomerId = i.CustomerId,
                Customer = i.Customer
            });

            return Ok(dtos);
        }

        // GET: api/Interaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InteractionDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando interação com id {InteractionId}", id);

            var interaction = await _interactionsRepository.GetByIdAsync(id);

            if (interaction == null)
            {
                _logger.LogWarning("Interação com id {InteractionId} não encontrada.", id);
                return NotFound();
            }

            return Ok(_mapper.Map<InteractionDto>(interaction));
        }

        // POST: api/Interaction
        [HttpPost]
        public async Task<ActionResult<InteractionDto>> Create(InteractionCreateDto dto)
        {
            _logger.LogInformation("Criando nova interação.");

            var interaction = _mapper.Map<Interaction>(dto);
            interaction.Date = DateTime.UtcNow;

            await _interactionsRepository.AddAsync(interaction);

            var interactionDto = _mapper.Map<InteractionDto>(interaction);

            return CreatedAtAction(nameof(GetById), new { id = interaction.Id }, interactionDto);
        }

        // PUT: api/Interaction/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InteractionUpdateDto dto)
        {
            _logger.LogInformation("Atualizando interação com id {InteractionId}", id);

            var interaction = await _interactionsRepository.GetByIdAsync(id);
            if (interaction == null)
            {
                _logger.LogWarning("Interação com id {InteractionId} não encontrada.", id);
                return NotFound();
            }

            _mapper.Map(dto, interaction);
            await _interactionsRepository.UpdateAsync(interaction);

            return NoContent();
        }

        // DELETE: api/Interaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Removendo interação com id {InteractionId}", id);

            var interaction = await _interactionsRepository.GetByIdAsync(id);
            if (interaction == null)
            {
                _logger.LogWarning("Interação com id {InteractionId} não encontrada.", id);
                return NotFound();
            }

            await _interactionsRepository.DeleteAsync(interaction);

            return NoContent();
        }
    }
}
