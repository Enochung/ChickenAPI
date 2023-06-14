using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChickenApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseServicesAsync _purchaseServicesAsync;

        public PurchasesController(PurchaseServicesAsync purchaseServicesAsync)
        {
            _purchaseServicesAsync = purchaseServicesAsync;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _purchaseServicesAsync.取得所有進貨資料();

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
        public async Task<IActionResult> GetSelect(PurchaseGetDto purchaseGetDto)
        {
            try
            {
                if (purchaseGetDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _purchaseServicesAsync.取得條件進貨資料(purchaseGetDto);

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

        [HttpPost]
        public async Task<IActionResult> Post(PurchasePostDto purchasePostDto)
        {
            try
            {
                if (purchasePostDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _purchaseServicesAsync.新增進貨資料(purchasePostDto);

                if(result.status == 404)
                {
                    return BadRequest(result.message);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PurchasePutDto purchasePutDto)
        {
            try
            {
                if (purchasePutDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _purchaseServicesAsync.修改進貨資料(purchasePutDto);

                if (result.status == 404)
                {
                    return UnprocessableEntity(result.message);
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
