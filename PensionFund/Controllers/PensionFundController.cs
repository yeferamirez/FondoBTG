using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PensionFund.Business.Services;
using PensionFund.Domain.Constants;
using PensionFund.Domain.Entities.Requests;
using Serilog;

namespace PensionFund.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class PensionFundController : ControllerBase
    {
        private readonly PensionFundService _pensionFundService;

        public PensionFundController(PensionFundService pensionFundService)
        {
            _pensionFundService = pensionFundService;
        }

        [HttpPost]
        [Route("subscribe-fund")]
        public async Task<IActionResult> SubcribeFund([FromBody] SubscribeFundRequest subscribeFundRequest)
        {
            try
            {
                var response = await _pensionFundService.SubcribeFund(subscribeFundRequest);
                if (response.Status.Equals(ResponseConstants.FAILED_PROCESS))
                    return BadRequest(response);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e)
            {
                Log.Error($"Error subscribe fund - {e.Message}");
                Log.Error(JsonConvert.SerializeObject(e));
                return BadRequest(JsonConvert.SerializeObject(e));
            }
        }

        [HttpPost]
        [Route("unsubscribe-fund")]
        public async Task<IActionResult> UnsubscribeFund([FromBody] UnsubscribeFundRequest unsubscribeFundRequest)
        {
            try
            {
                var response = await _pensionFundService.UnsubscribeFund(unsubscribeFundRequest);
                if (response.Status.Equals(ResponseConstants.FAILED_PROCESS))
                    return BadRequest(response);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e)
            {
                Log.Error($"Error unsubscribe fund - {e.Message}");
                Log.Error(JsonConvert.SerializeObject(e));
                return BadRequest(JsonConvert.SerializeObject(e));
            }
        }

        [HttpPost]
        [Route("get-clients")]
        public async Task<IActionResult> GetClients(string city)
        {
            try
            {
                var response = await _pensionFundService.GetClients(city);
                if (response.Count == 0)
                    return BadRequest(ExceptionConstants.NOT_EXIST_CLIENT_CITY);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e)
            {
                Log.Error($"Error unsubscribe fund - {e.Message}");
                Log.Error(JsonConvert.SerializeObject(e));
                return BadRequest(JsonConvert.SerializeObject(e));
            }
        }

        [HttpGet]
        [Route("list-transactions")]
        public async Task<IActionResult> GetTransactions(string date)
        {
            try
            {
                var response = await _pensionFundService.GetTransactions(date);
                if (response.Status.Equals(ResponseConstants.FAILED_PROCESS))
                    return BadRequest(response);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e)
            {
                Log.Error($"Error unsubscribe fund - {e.Message}");
                Log.Error(JsonConvert.SerializeObject(e));
                return BadRequest(JsonConvert.SerializeObject(e));
            }
        }

        [HttpGet]
        [Route("get-fundconfiguration")]
        public async Task<IActionResult> GetFundconfiguration()
        {
            try
            {
                var response = await _pensionFundService.GetFundconfiguration();
                if (response.Status.Equals(ResponseConstants.FAILED_PROCESS))
                    return BadRequest(response);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e)
            {
                Log.Error($"Error unsubscribe fund - {e.Message}");
                Log.Error(JsonConvert.SerializeObject(e));
                return BadRequest(JsonConvert.SerializeObject(e));
            }
        }
    }
}
