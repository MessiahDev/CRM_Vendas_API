using System.Security.Claims;
using AutoMapper;
using CRM_Vendas.Application.DTOs.UserDto;
using CRM_Vendas.Application.Interfaces;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Entities.DTOs.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IAuthenticationService authenticationService,
            IUserRepository userRepository,
            IPasswordService passwordService)
        {
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            _logger.LogInformation("Tentativa de login para o e-mail {Email}", dto.Email);

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado para o e-mail {Email}", dto.Email);
                return Unauthorized("E-mail ou senha inválidos.");
            }

            var token = await _authenticationService.AuthenticateAsync(dto.Email, dto.Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Senha incorreta para o e-mail {Email}", dto.Email);
                return Unauthorized("E-mail ou senha inválidos.");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new
            {
                User = userDto,
                Token = token
            });
        }

        // GET: api/User/me
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> ValidateToken()
        {
            var email = User?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Token sem e-mail ou usuário não autenticado.");
                return Unauthorized("Token inválido ou expirado.");
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado para o e-mail extraído do token: {Email}", email);
                return Unauthorized("Usuário não encontrado.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            _logger.LogInformation("Tentativa de registro para o e-mail: {Email}", dto.Email);

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("E-mail já em uso: {Email}", dto.Email);
                return BadRequest("E-mail já está em uso.");
            }

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = _passwordService.HashPassword(dto.Password);

            await _userRepository.AddAsync(user);

            var token = await _authenticationService.AuthenticateAsync(user.Email, dto.Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError("Erro ao gerar token após registro para o e-mail: {Email}", user.Email);
                return StatusCode(500, "Erro ao gerar token.");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new
            {
                User = userDto,
                Token = token
            });
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os usuários.");
            var users = await _userRepository.GetAllAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando usuário com id {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Usuário com id {UserId} não encontrado.", id);
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST: api/User
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserCreateDto dto)
        {
            _logger.LogInformation("Criando novo usuário.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = HashPassword(dto.Password);

            await _userRepository.AddAsync(user);

            var userDto = _mapper.Map<UserDto>(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
        }

        // PUT: api/User/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            _logger.LogInformation("Atualizando usuário com id {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Usuário com id {UserId} não encontrado para atualização.", id);
                return NotFound("Usuário não encontrado.");
            }

            _mapper.Map(dto, user);

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = _passwordService.HashPassword(dto.Password);
                _logger.LogInformation("Senha atualizada para o usuário com id {UserId}", id);
            }

            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário com id {UserId}", id);
                return StatusCode(500, "Erro ao atualizar usuário.");
            }

            return NoContent();
        }

        // DELETE: api/User/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Excluindo usuário com id {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Usuário com id {UserId} não encontrado para exclusão.", id);
                return NotFound();
            }

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
