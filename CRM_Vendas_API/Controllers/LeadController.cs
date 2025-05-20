using AutoMapper;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas_API.Context;
using CRM_Vendas_API.Entities.DTOs.LeadDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LeadController> _logger;
        private readonly IMapper _mapper;

        public LeadController(AppDbContext context, ILogger<LeadController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Lead
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os leads.");
            var leads = await _context.Leads
                .Include(l => l.Customer)
                .Include(l => l.User)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<LeadDto>>(leads));
        }

        // GET: api/Lead/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeadDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando lead com id {LeadId}", id);
            var lead = await _context.Leads
                .Include(l => l.Customer)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lead == null)
            {
                _logger.LogWarning("Lead com id {LeadId} não encontrado.", id);
                return NotFound();
            }

            return Ok(_mapper.Map<LeadDto>(lead));
        }

        // POST: api/Lead
        [HttpPost]
        public async Task<ActionResult<LeadDto>> CreateLead(LeadCreateDto dto)
        {
            _logger.LogInformation("Criando novo lead.");

            var lead = _mapper.Map<Lead>(dto);
            lead.CreatedAt = DateTime.UtcNow;

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            var leadDto = _mapper.Map<LeadDto>(lead);

            return CreatedAtAction(nameof(GetById), new { id = lead.Id }, leadDto);
        }

        // PUT: api/Lead/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LeadUpdateDto dto)
        {
            _logger.LogInformation("Atualizando lead com id {LeadId}", id);

            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                _logger.LogWarning("Lead com id {LeadId} não encontrado.", id);
                return NotFound();
            }

            _mapper.Map(dto, lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Lead/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Excluindo lead com id {LeadId}", id);

            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                _logger.LogWarning("Lead com id {LeadId} não encontrado.", id);
                return NotFound();
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
