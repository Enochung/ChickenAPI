using ChickenApplication.Dtos.StocksDtos;
using ChickenApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChickenApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly StockServicesAsync _stockServicesAsync;

        public StocksController(StockServicesAsync stockServicesAsync)
        {
            _stockServicesAsync = stockServicesAsync;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _stockServicesAsync.取得所有庫存資料Async();

                if (result.status == 404)
                {
                    return NotFound("找不到資料");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GetSelect")]
        public async Task<IActionResult> GetSelect(StockGetDto stockGetDto)
        {
            try
            {
                if (stockGetDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _stockServicesAsync.取得條件庫存資料Async(stockGetDto);

                if (result.status == 404)
                {
                    return NotFound("找不到資料");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
