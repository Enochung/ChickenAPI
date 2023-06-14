using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Dtos.SuppliersDtos;
using ChickenApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChickenApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierServiceAsync _supplierServiceAsync;

        public SuppliersController(SupplierServiceAsync supplierServiceAsync)
        {
            _supplierServiceAsync = supplierServiceAsync;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _supplierServiceAsync.取得所有廠商資料();

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
        public async Task<IActionResult> GetSelect(SupplierGetDto supplierGetDto)
        {
            try
            {
                if (supplierGetDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _supplierServiceAsync.取得條件廠商資料(supplierGetDto);

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
        public async Task<IActionResult> Post(SupplierPostDto supplierPostDto)
        {
            try
            {
                if (supplierPostDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _supplierServiceAsync.新增廠商資料(supplierPostDto);

                if (result.status == 404)
                {
                    return BadRequest(result.message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(SupplierPutDto supplierPutDto)
        {
            try
            {
                if (supplierPutDto == null)
                {
                    return BadRequest("請求錯誤");
                }

                var result = await _supplierServiceAsync.修改廠商資料(supplierPutDto);

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
