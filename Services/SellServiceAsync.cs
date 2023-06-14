using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Dtos.SellsDtos;
using ChickenApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ChickenApplication.Services
{
    public class SellServiceAsync
    {
        private readonly ChickenContext _chickenContext;

        public SellServiceAsync(ChickenContext chickenContext)
        {
            _chickenContext = chickenContext;
        }

        // 查詢
        public async Task<List<SellTableWithItemName>> 取得所有銷售資料()
        {
            //var sellTable = await _chickenContext.SellTables.ToListAsync();

            var result = await _chickenContext.SellTables
                .Join(_chickenContext.ItemTables,
                sellTable => sellTable.ItemId,
                itemTable => itemTable.ItemId,
                (sellTable, itemTable) => new SellTableWithItemName
                {
                    TableId = sellTable.TableId,
                    ItemName = itemTable.ItemName,
                    SellQuantity = sellTable.SellQuantity,
                    SellPrice = sellTable.SellPrice,
                    AddDate = sellTable.AddDate,
                    RenewDate = sellTable.RenewDate,
                }).ToListAsync();

            return result;
        }

        // 有條件查詢
        public async Task<List<SellTableWithItemName>> 取得條件銷售資料(SellGetDto sellGetDto)
        {
            var sellTable = _chickenContext.SellTables.AsQueryable();

            sellTable = 過濾SellsGetDto資料(sellTable, sellGetDto);

            var result = await sellTable
                .Join(_chickenContext.ItemTables,
                sellTable => sellTable.ItemId,
                itemTable => itemTable.ItemId,
                (sellTable, itemTable) => new SellTableWithItemName
                {
                    TableId = sellTable.TableId,
                    ItemName = itemTable.ItemName,
                    SellQuantity = sellTable.SellQuantity,
                    SellPrice = sellTable.SellPrice,
                    AddDate = sellTable.AddDate,
                    RenewDate = sellTable.RenewDate,
                })
                .ToListAsync();

            return result;
        }

        // 新增
        public async Task<SellTable> 新增銷售資料(SellPostDto sellPostDto)
        {
            var sellTable = new SellTable
            {
                ItemId = sellPostDto.ItemId,
                SellQuantity = sellPostDto.SellQuantity,
                SellPrice = sellPostDto.SellPrice,
                AddDate = new DateTime().Date,
                RenewDate = new DateTime().Date,
            };

            _chickenContext.SellTables.Add(sellTable);

            var stockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == sellPostDto.ItemId);

            if (stockTable != null && stockTable.ItemStock > 0)
            {
                stockTable.ItemStock -= sellPostDto.SellQuantity;

                stockTable.RenewDate = new DateTime().Date;

                _chickenContext.StockTables.Update(stockTable);
            }

            await _chickenContext.SaveChangesAsync();

            return sellTable;
        }

        // 修改
        // 待處理
        public async Task<SellTable?> 修改銷售資料(SellPutDto sellPutDto)
        {
            var sellTable = await _chickenContext.SellTables.SingleOrDefaultAsync(a => a.TableId == sellPutDto.TableId);

            if (sellTable == null)
            {
                return null;
            }

            //只更改ItemId => 舊ItemId的庫存數量要減少，新ItemId的庫存數量要增加 OK
            //同時更改ItemId與SellQuantity => 舊ItemId的庫存數量要減少，新ItemId的庫存數量要增加
            if (sellPutDto.ItemId.HasValue && sellPutDto.ItemId.Value != sellTable.ItemId)
            {
                await ItemId不同時更新庫存資料(sellTable, sellPutDto);
            }

            //只更改SellQuantity => 新的數量大於舊的數量ItemId的庫存數量要增加，新的數量小於舊的數量ItemId的庫存數量要減少
            if (!sellPutDto.ItemId.HasValue && sellPutDto.SellQuantity.HasValue && sellPutDto.SellQuantity.Value != sellTable.SellQuantity)
            {
                await SellQuantity不同時更新庫存資料(sellTable, sellPutDto);
            }

            sellTable = 過濾SellPutDt資料(sellTable, sellPutDto);

            sellTable.RenewDate = new DateTime().Date;

            await _chickenContext.SaveChangesAsync();

            return sellTable;
        }

        // 過濾
        private IQueryable<SellTable> 過濾SellsGetDto資料(IQueryable<SellTable> sellTable, SellGetDto sellsGetDto)
        {
            if (sellsGetDto.TableId.HasValue && sellsGetDto.TableId.Value > 0)
            {
                sellTable = sellTable.Where(a => a.TableId == sellsGetDto.TableId);
            }

            if (sellsGetDto.ItemId.HasValue)
            {
                sellTable = sellTable.Where(a => a.ItemId == sellsGetDto.ItemId.Value);
            }

            if (sellsGetDto.SellQuantity.HasValue && sellsGetDto.SellQuantity.Value > 0)
            {
                sellTable = sellTable.Where(a => a.SellQuantity == sellsGetDto.SellQuantity);
            }

            if (sellsGetDto.SellPrice.HasValue && sellsGetDto.SellPrice.Value > 0)
            {
                sellTable = sellTable.Where(a => a.SellPrice == sellsGetDto.SellPrice);
            }

            if (sellsGetDto.AddDate.HasValue)
            {
                sellTable = sellTable.Where(a => a.AddDate == sellsGetDto.AddDate);
            }

            if (sellsGetDto.RenewDate.HasValue)
            {
                sellTable = sellTable.Where(a => a.RenewDate == sellsGetDto.RenewDate);
            }

            return sellTable;
        }

        private SellTable 過濾SellPutDt資料(SellTable sellTable, SellPutDto sellPutDto)
        {
            if (sellPutDto.ItemId.HasValue)
            {
                sellTable.ItemId = sellPutDto.ItemId.Value;
            }

            if (sellPutDto.SellQuantity.HasValue && sellPutDto.SellQuantity > 0)
            {
                sellTable.SellQuantity = sellPutDto.SellQuantity.Value;
            }

            if (sellPutDto.SellPrice.HasValue && sellPutDto.SellPrice > 0)
            {
                sellTable.SellPrice = sellPutDto.SellPrice.Value;
            }

            return sellTable;
        }

        private async Task ItemId不同時更新庫存資料(SellTable sellTable, SellPutDto sellPutDto)
        {
            var oldStockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == sellTable.ItemId);

            if (oldStockTable != null)
            {
                oldStockTable.ItemStock -= sellTable.SellQuantity;

                oldStockTable.RenewDate = new DateTime().Date;

                _chickenContext.StockTables.Update(oldStockTable);
            }

            var newStockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == sellPutDto.ItemId);

            if (newStockTable != null)
            {
                newStockTable.ItemStock += sellTable.SellQuantity;

                newStockTable.RenewDate = new DateTime().Date;

                _chickenContext.StockTables.Update(newStockTable);
            }
        }

        private async Task SellQuantity不同時更新庫存資料(SellTable sellTable, SellPutDto sellPutDto)
        {
            var stockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == sellTable.ItemId);

            if (stockTable != null)
            {
                stockTable.ItemStock += (int)(sellPutDto.SellQuantity - sellTable.SellQuantity);

                stockTable.RenewDate = new DateTime().Date;

                _chickenContext.StockTables.Update(stockTable);
            }
        }
    }
}
