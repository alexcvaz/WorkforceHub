using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkforceHub.Server.Application.DTOs;
using WorkforceHub.Server.Application.Interfaces;

namespace WorkforceHub.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        private readonly ILogger<PunchController> _logger;

        public PunchController(IPunchService punchService, ILogger<PunchController> logger)
        {
            _punchService = punchService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<PunchResponse>> RegisterPunch(
            [FromBody] CreatePunchRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Punch registration attempted with no user ID");
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers.UserAgent.ToString();

                var response = await _punchService.RegisterPunchAsync(
                    userId,
                    request,
                    ipAddress,
                    userAgent,
                    cancellationToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering punch for user {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error registering punch");
            }
        }

        [HttpGet("today")]
        public async Task<ActionResult<List<PunchResponse>>> GetTodayPunches(CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Get punches attempted with no user ID");
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                var response = await _punchService.GetTodayPunchesAsync(userId, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving punches for user {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving punches");
            }
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<List<PunchResponse>>> GetPunchesByDate(
            string date,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Get punches attempted with no user ID");
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                if (!DateOnly.TryParse(date, out var parsedDate))
                {
                    return BadRequest("Invalid date format. Use yyyy-MM-dd");
                }

                var response = await _punchService.GetPunchesByDateAsync(userId, parsedDate, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving punches for user {UserId} and date {Date}", userId, date);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving punches");
            }
        }

        [HttpGet("range")]
        public async Task<ActionResult<List<PunchResponse>>> GetPunchesByDateRange(
            [FromQuery] string startDate,
            [FromQuery] string endDate,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Get punches attempted with no user ID");
                return Unauthorized("User ID not found in claims");
            }

            try
            {
                if (!DateOnly.TryParse(startDate, out var parsedStartDate))
                {
                    return BadRequest("Invalid startDate format. Use yyyy-MM-dd");
                }

                if (!DateOnly.TryParse(endDate, out var parsedEndDate))
                {
                    return BadRequest("Invalid endDate format. Use yyyy-MM-dd");
                }

                if (parsedStartDate > parsedEndDate)
                {
                    return BadRequest("startDate must be less than or equal to endDate");
                }

                var response = await _punchService.GetPunchesByDateRangeAsync(
                    userId,
                    parsedStartDate,
                    parsedEndDate,
                    cancellationToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving punches for user {UserId} in range {StartDate}-{EndDate}", 
                    userId, startDate, endDate);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving punches");
            }
        }
    }
}
