using AutoMapper;
using CRM_Vendas.Application.DTOs.UserDto;
using CRM_Vendas.Application.Interfaces;
using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Entities.DTOs.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPasswordDto dto)
        {
            _logger.LogInformation("Solicitação de redefinição de senha recebida para: {Email}", dto.Email);

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("E-mail não encontrado: {Email}", dto.Email);
                return Ok(new { Message = "Se o e-mail estiver cadastrado, você receberá instruções para redefinir sua senha." });
            }

            var resetToken = _authenticationService.GenerateToken(user.Name, user.Email, user.Role.ToString());
            user.PasswordResetToken = resetToken;

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Token de redefinição de senha para {Email}: {Token}", dto.Email, resetToken);

            return Ok(new { Message = "Token gerado com sucesso.", Token = resetToken });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto dto)
        {
            _logger.LogInformation("Solicitação de reset de senha recebida com token.");

            var user = await _userRepository.GetByResetTokenAsync(dto.Token);
            if (user == null)
            {
                _logger.LogWarning("Token inválido ou expirado: {Token}", dto.Token);
                return BadRequest("Token inválido ou expirado.");
            }

            user.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Senha redefinida com sucesso para {Email}.", user.Email);

            return Ok(new { Message = "Senha redefinida com sucesso." });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            _logger.LogInformation("Buscando todos os usuários.");
            var users = await _userRepository.GetAllAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }

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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserCreateDto dto)
        {
            _logger.LogInformation("Criando novo usuário.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = _passwordService.HashPassword(dto.Password);

            await _userRepository.AddAsync(user);

            var userDto = _mapper.Map<UserDto>(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
        }

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

            await _userRepository.DeleteAsync(user);

            return NoContent();
        }
    }
}
