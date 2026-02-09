using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WorkforceHub.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // NOTA: Em produção, validar contra banco de dados
            // Para desenvolvimento, aceitar qualquer usuário
            if (string.IsNullOrEmpty(request.UserId))
            {
                _logger.LogWarning("Login attempt with empty userId");
                return BadRequest("UserId is required");
            }

            try
            {
                var token = GenerateJwtToken(request.UserId);
                _logger.LogInformation("User {UserId} logged in successfully", request.UserId);
                
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user {UserId}", request.UserId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error generating token");
            }
        }

        private string GenerateJwtToken(string userId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not found");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(24), // Token válido por 24h
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string UserId { get; set; } = string.Empty;
        // Opcional: adicionar password em produção
        // public string Password { get; set; } = string.Empty;
    }
}
