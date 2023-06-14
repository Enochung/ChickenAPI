using ChickenApplication.Dtos.SellsDtos;
using ChickenApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChickenApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellsController : ControllerBase
    {
        private readonly SellServiceAsync _sellServiceAsync;

        public SellsController(SellServiceAsync sellServiceAsync)
        {
            _sellServiceAsync = sellServiceAsync;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _sellServiceAsync.取得所有銷售資料();

                if (result == null)
                {
                    return NotFound("找不到資料");
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetSelect")]
        public async Task<IActionResult> GetSelect(SellGetDto sellGetDto)
        {
            try
            {
                if (sellGetDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _sellServiceAsync.取得條件銷售資料(sellGetDto);

                if (result == null)
                {
                    return NotFound("找不到資料");
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(SellPostDto sellPostDto)
        {
            try
            {
                if (sellPostDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _sellServiceAsync.新增銷售資料(sellPostDto);

                if (result == null)
                {
                    return NotFound("新增失敗");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(SellPutDto sellPutDto)
        {
            try
            {
                if (sellPutDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _sellServiceAsync.修改銷售資料(sellPutDto);

                if (result == null)
                {
                    return UnprocessableEntity("修改失敗");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
