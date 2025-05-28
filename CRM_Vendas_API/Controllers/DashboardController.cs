using CRM_Vendas.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace CRM_Vendas_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(ILogger<DashboardController> logger, IDashboardRepository dashboardRepository)
        {
            _logger = logger;
            _dashboardRepository = dashboardRepository;
        }

        // GET: api/Dashboard
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            _logger.LogInformation("Iniciando obtenção dos dados do dashboard.");
            try
            {
                var (deals, leads, interactions, customers) = await _dashboardRepository.GetAllAsync();
                _logger.LogInformation("Dados do dashboard obtidos com sucesso. Negócios: {DealsCount}, Leads: {LeadsCount}, Interações: {InteractionsCount}, Clientes: {CustomersCount}",
                    deals.Count, leads.Count, interactions.Count, customers.Count);

                return Ok(new
                {
                    Deals = deals,
                    Leads = leads,
                    Interactions = interactions,
                    Customers = customers
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter dados do dashboard.");
                return StatusCode(500, "Erro interno ao obter dados do dashboard.");
            }
        }
    }
}
