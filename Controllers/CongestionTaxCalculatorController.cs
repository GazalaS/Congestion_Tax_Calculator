using Congestion.Calculator.Model;
using Congestion.Calculator.Service;
using Microsoft.AspNetCore.Mvc;

namespace Congestion.Calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CongestionTaxCalculatorController : ControllerBase
    {

        private readonly ILogger<CongestionTaxCalculatorController> _logger;
        private readonly ICongestionTaxCalculatorService _congestionTaxCalculatorService;

        public CongestionTaxCalculatorController(ILogger<CongestionTaxCalculatorController> logger, ICongestionTaxCalculatorService congestionTaxCalculatorService)
        {
            _logger = logger;
            _congestionTaxCalculatorService = congestionTaxCalculatorService;

        }

        [HttpGet(Name = "GetTax")]
        public async Task<IActionResult> Get([FromQuery] Vehicle vehicle, [FromQuery] List<DateTime> dates, [FromQuery] string cityName)
        {

            try
            {
                if (vehicle is null)
                {
                    _logger.LogError("Vehicle object sent from client is null.");
                    return BadRequest("Vehicle object is null");
                }
                if (dates is null)
                {
                    _logger.LogError("Dates sent from client is null.");
                    return BadRequest("Dates is null");
                }
                if (cityName is null)
                {
                    _logger.LogError("cityName sent from client is null.");
                    return BadRequest("cityName is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var data = await _congestionTaxCalculatorService.GetTax(vehicle, dates, cityName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetTax action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }          
        }
    }
}