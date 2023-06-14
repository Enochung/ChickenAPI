using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Dtos.TemporarystocksDtos;
using ChickenApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ChickenApplication.Services
{
    public class TemporarystockServiceAsync
    {
        private readonly ChickenContext _chickenContext;
        public TemporarystockServiceAsync(ChickenContext chickenContext)
        {
            _chickenContext = chickenContext;
        }

        public async Task<ResponseMessage> 取得所有暫存庫存資料()
        {
            var result = await _chickenContext.TemporarystockTables
                .Join(_chickenContext.ItemTables,
                temporarystockTable => temporarystockTable.ItemId,
                itemTable => itemTable.ItemId,
                (temporarystockTable, itemTable) => new TemporarystockTableWithItemName
                {
                    TableId = temporarystockTable.TableId,
                    ItemName = itemTable.ItemName,
                    ItemTemporarystock = temporarystockTable.ItemTemporarystock,
                    AddDate = temporarystockTable.AddDate,
                    RenewDate = temporarystockTable.RenewDate,
                }).ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        public async Task<ResponseMessage> 取得條件暫存庫存資料(TemporarystockGetDto temporarystockGetDto)
        {
            var temporarystockTable = _chickenContext.TemporarystockTables.AsQueryable();

            temporarystockTable = 過濾TemporarystockGetDto資料(temporarystockTable, temporarystockGetDto);

            var result = await temporarystockTable
                .Join(_chickenContext.ItemTables,
                temporarystock => temporarystock.ItemId,
                item => item.ItemId,
                (temporarystock, item) => new TemporarystockTableWithItemName
                {
                    TableId = temporarystock.TableId,
                    ItemName = item.ItemName,
                    ItemTemporarystock = temporarystock.ItemTemporarystock,
                    AddDate = temporarystock.AddDate,
                    RenewDate = temporarystock.RenewDate,
                }).ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }


        public async Task<ResponseMessage> 新增暫存庫存資料()
        {
            return null;
        }

        private IQueryable<TemporarystockTable> 過濾TemporarystockGetDto資料(IQueryable<TemporarystockTable> temporarystockTable, TemporarystockGetDto temporarystockGetDto)
        {
            if (temporarystockGetDto.TableId.HasValue && temporarystockGetDto.TableId.Value > 0)
            {
                temporarystockTable = temporarystockTable.Where(a => a.TableId == temporarystockGetDto.TableId);
            }

            if (!string.IsNullOrWhiteSpace(temporarystockGetDto.ItemName))
            {
                var itemIds = _chickenContext.ItemTables
                    .Where(a => a.ItemName == temporarystockGetDto.ItemName)
                    .Select(a => a.ItemId)
                    .ToList();

                temporarystockTable = temporarystockTable.Where(a => itemIds.Contains(a.ItemId));
            }

            if (temporarystockGetDto.ItemTemporarystock.HasValue && temporarystockGetDto.ItemTemporarystock.Value > 0)
            {
                temporarystockTable = temporarystockTable.Where(a => a.ItemTemporarystock == temporarystockGetDto.ItemTemporarystock);
            }

            if (temporarystockGetDto.AddDate.HasValue)
            {
                temporarystockTable = temporarystockTable.Where(a => a.AddDate == temporarystockGetDto.AddDate);
            }

            if (temporarystockGetDto.RenewDate.HasValue)
            {
                temporarystockTable = temporarystockTable.Where(a => a.RenewDate == temporarystockGetDto.RenewDate);
            }

            return temporarystockTable;
        }
    }
}
