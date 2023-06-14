using ChickenApplication.Dtos.ItemsDtos;
using ChickenApplication.Models;
using ChickenApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChickenApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly ItemServicesAsync _itemServicesAsync;

        public ItemsController(ItemServicesAsync itemServicesAsync)
        {
            _itemServicesAsync = itemServicesAsync;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _itemServicesAsync.取得所有品項資料Async();

                if (result.status == 404)
                {
                    return NotFound(result.message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetSelect")]
        public async Task<IActionResult> GetSelect(ItemGetDto itemGetDto)
        {
            try
            {
                if (itemGetDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _itemServicesAsync.取得條件品項資料Async(itemGetDto);

                if (result.status == 404)
                {
                    return NotFound(result.message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ItemPostDto itemPostDto)
        {
            try
            {

                if (itemPostDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _itemServicesAsync.新增品項資料Async(itemPostDto);

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
        public async Task<IActionResult> Put(ItemPutDto itemPutDto)
        {
            try
            {
                if (itemPutDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _itemServicesAsync.修改品項資料(itemPutDto);

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
