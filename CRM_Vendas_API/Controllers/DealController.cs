using AutoMapper;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas_API.Context;
using CRM_Vendas_API.Entities.DTOs.DealDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DealController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DealController> _logger;
        private readonly IMapper _mapper;

        public DealController(AppDbContext context, ILogger<DealController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Deal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os negócios.");
            var deals = await _context.Deals
                .Include(d => d.Customer)
                .Include(d => d.Lead)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        // GET: api/Deal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DealDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando negócio com id {DealId}", id);

            var deal = await _context.Deals
                .Include(d => d.Customer)
                .Include(d => d.Lead)
                .FirstOrDefaultAsync(d => d.Id == id);

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

            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();

            var dealDto = _mapper.Map<DealDto>(deal);

            return CreatedAtAction(nameof(GetById), new { id = deal.Id }, dealDto);
        }

        // PUT: api/Deal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DealUpdateDto dto)
        {
            _logger.LogInformation("Atualizando negócio com id {DealId}", id);

            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                _logger.LogWarning("Negócio com id {DealId} não encontrado.", id);
                return NotFound();
            }

            _mapper.Map(dto, deal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Deal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Removendo negócio com id {DealId}", id);

            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                _logger.LogWarning("Negócio com id {DealId} não encontrado.", id);
                return NotFound();
            }

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
