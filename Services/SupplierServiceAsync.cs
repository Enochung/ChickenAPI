using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Dtos.StocksDtos;
using ChickenApplication.Dtos.SuppliersDtos;
using ChickenApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace ChickenApplication.Services
{
    public class SupplierServiceAsync
    {
        private readonly ChickenContext _chickenContext;

        public SupplierServiceAsync(ChickenContext chickenContext)
        {
            _chickenContext = chickenContext;
        }

        // 搜尋
        public async Task<ResponseMessage> 取得所有廠商資料()
        {
            var result = await _chickenContext.SupplierTables.ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        // 有條件搜尋
        public async Task<ResponseMessage> 取得條件廠商資料(SupplierGetDto supplierGetDto)
        {
            var supplierTable = _chickenContext.SupplierTables.AsQueryable();

            supplierTable = 過濾SupplierGetDto資料(supplierTable, supplierGetDto);

            var result = await supplierTable.ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        // 新增
        public async Task<ResponseMessage> 新增廠商資料(SupplierPostDto supplierPostDto)
        {
            var supplierTable = new SupplierTable
            {
                SupplierId = Guid.NewGuid(),
                SupplierName = supplierPostDto.SupplierName,
                SupplierPhone = supplierPostDto.SupplierPhone,
                SupplierAddress = supplierPostDto.SupplierAddress,
                AddDate = new DateTime().Date,
                RenewDate = new DateTime().Date,
            };

            _chickenContext.SupplierTables.Add(supplierTable);

            await _chickenContext.SaveChangesAsync();

            return new ResponseMessage("新增成功", 200, supplierTable);
        }

        // 修改
        public async Task<ResponseMessage> 修改廠商資料(SupplierPutDto supplierPutDto)
        {
            var supplierTable = await _chickenContext.SupplierTables.SingleOrDefaultAsync(a => a.SupplierId == supplierPutDto.SupplierId);

            if (supplierTable == null)
            {
                return new ResponseMessage("找不到對應的供應商", 404);
            }

            supplierTable.RenewDate = new DateTime().Date;

            _chickenContext.SupplierTables.Update(supplierTable).CurrentValues.SetValues(supplierPutDto);

            await _chickenContext.SaveChangesAsync();

            return new ResponseMessage("修改成功", 200, supplierTable);
        }

        // 過濾
        private IQueryable<SupplierTable> 過濾SupplierGetDto資料(IQueryable<SupplierTable> supplierTable, SupplierGetDto supplierGetDto)
        {
            if (supplierGetDto.TableId.HasValue && supplierGetDto.TableId.Value > 0)
            {
                supplierTable = supplierTable.Where(a => a.TableId == supplierGetDto.TableId);
            }

            if (supplierGetDto.SupplierId.HasValue)
            {
                supplierTable = supplierTable.Where(a => a.SupplierId == supplierGetDto.SupplierId);
            }

            if (!string.IsNullOrEmpty(supplierGetDto.SupplierName))
            {
                supplierTable = supplierTable.Where(a => a.SupplierName == supplierGetDto.SupplierName);
            }

            if (!string.IsNullOrEmpty(supplierGetDto.SupplierPhone))
            {
                supplierTable = supplierTable.Where(a => a.SupplierPhone == supplierGetDto.SupplierPhone);
            }

            if (!string.IsNullOrEmpty(supplierGetDto.SupplierAddress))
            {
                supplierTable = supplierTable.Where(a => a.SupplierAddress == supplierGetDto.SupplierAddress);
            }

            if (supplierGetDto.AddDate.HasValue)
            {
                supplierTable = supplierTable.Where(a => a.AddDate == supplierGetDto.AddDate);
            }

            if (supplierGetDto.RenewDate.HasValue)
            {
                supplierTable = supplierTable.Where(a => a.RenewDate == supplierGetDto.RenewDate);
            }

            return supplierTable;
        }
    }
}
