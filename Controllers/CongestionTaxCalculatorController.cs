using Congestion.Calculator.Model;
using Congestion.Calculator.Service;
using Microsoft.AspNetCore.Mvc;

namespace Congestion.Calculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CongestionTaxCalculatorController : ControllerBase
    {
        private readonly ILogger<CongestionTaxCalculatorController> _logger;
        private readonly ICongestionTaxCalculatorService _taxService;

        public CongestionTaxCalculatorController(ILogger<CongestionTaxCalculatorController> logger, ICongestionTaxCalculatorService taxService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _taxService = taxService ?? throw new ArgumentNullException(nameof(taxService));
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> GetTax([FromQuery] Vehicle vehicle, [FromQuery] List<DateTime> dates, [FromQuery] string cityName)
        {
            try
            {
                // Validate inputs
                if (vehicle == null)
                    return BadRequest(new { Message = "Vehicle object is null" });

                if (dates == null || !dates.Any())
                    return BadRequest(new { Message = "Dates list is null or empty" });

                if (string.IsNullOrWhiteSpace(cityName))
                    return BadRequest(new { Message = "City name is null or empty" });

                if (!ModelState.IsValid)
                    return BadRequest(new { Message = "Invalid model object", Details = GetModelErrors() });

                // Perform calculation
                var taxAmount = await _taxService.GetTax(vehicle, dates, cityName);

                return Ok(new
                {
                    Success = true,
                    Data = new
                    {
                        City = cityName,
                        TotalTax = taxAmount
                    },
                    Message = "Tax calculated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while calculating tax: {ex.Message}", ex);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        private IEnumerable<string> GetModelErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        }
    }
}
