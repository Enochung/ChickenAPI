using ChickenApplication.Dtos.ItemsDtos;
using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Dtos.StocksDtos;
using ChickenApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ChickenApplication.Services
{
    public class StockServicesAsync
    {

        private readonly ChickenContext _chickenContext;

        public StockServicesAsync(ChickenContext chickenContext)
        {
            _chickenContext = chickenContext;
        }

        // 查詢
        public async Task<ResponseMessage> 取得所有庫存資料Async()
        {
            //var stockTable = await _chickenContext.StockTables.ToListAsync();

            var result = await _chickenContext.StockTables
                .Join(_chickenContext.ItemTables,
                stockTable => stockTable.ItemId,
                itemTable => itemTable.ItemId,
                (stockTable, itemTable) => new stockTableWithItemName
                {
                    TableId = stockTable.TableId,
                    ItemName = itemTable.ItemName,
                    ItemStock = stockTable.ItemStock,
                    AddDate = stockTable.AddDate,
                    RenewDate = stockTable.RenewDate,
                }).ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        // 有條件查詢
        public async Task<ResponseMessage> 取得條件庫存資料Async(StockGetDto stockGetDto)
        {
            var stockTable = _chickenContext.StockTables.AsQueryable();

            stockTable = 過濾StockGetDto資料(stockTable, stockGetDto);

            var result = await stockTable
                .Join(_chickenContext.ItemTables,
                stock => stock.ItemId,
                item => item.ItemId,
                (stock, item) => new stockTableWithItemName
                {
                    TableId = stock.TableId,
                    ItemName = item.ItemName,
                    ItemStock = stock.ItemStock,
                    AddDate = stock.AddDate,
                    RenewDate = stock.RenewDate,
                }).ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        // 過濾
        private IQueryable<StockTable> 過濾StockGetDto資料(IQueryable<StockTable> stockTable, StockGetDto stockGetDto)
        {
            if (stockGetDto.TableId.HasValue && stockGetDto.TableId.Value > 0)
            {
                stockTable = stockTable.Where(a => a.TableId == stockGetDto.TableId);
            }

            if (stockGetDto.ItemId.HasValue)
            {
                stockTable = stockTable.Where(a => a.ItemId == stockGetDto.ItemId.Value);
            }

            if (stockGetDto.ItmeStock.HasValue && stockGetDto.ItmeStock.Value > 0)
            {
                stockTable = stockTable.Where(a => a.ItemStock == stockGetDto.ItmeStock);
            }

            if (stockGetDto.AddDate.HasValue && stockGetDto.AddDate > DateTime.Now)
            {
                stockTable = stockTable.Where(a => a.AddDate == stockGetDto.AddDate);
            }

            if (stockGetDto.RenewDate.HasValue && stockGetDto.RenewDate > DateTime.Now)
            {
                stockTable = stockTable.Where(a => a.RenewDate == stockGetDto.RenewDate);
            }

            return stockTable;
        }
    }
}
