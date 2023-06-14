using ChickenApplication.Dtos.ItemsDtos;
using ChickenApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ChickenApplication.Services
{
    public class ItemServicesAsync
    {
        private readonly ChickenContext _chickenContext;

        public ItemServicesAsync(ChickenContext chickenContext)
        {
            _chickenContext = chickenContext;
        }

        // 搜尋
        public async Task<ResponseMessage> 取得所有品項資料Async()
        {

            var itemTable = await _chickenContext.ItemTables.ToListAsync();

            if (itemTable == null)
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, itemTable);

        }

        // 有條件搜尋
        public async Task<ResponseMessage> 取得條件品項資料Async(ItemGetDto itemGetDto)
        {
            var itemTable = _chickenContext.ItemTables.AsQueryable();

            itemTable = 過濾ItemGetDto資料(itemTable, itemGetDto);

            var result = await itemTable.ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        // 新增
        public async Task<ItemTable> 新增品項資料Async(ItemPostDto itemPostDto)
        {
            var itemTable = new ItemTable
            {
                ItemId = Guid.NewGuid(),
                ItemName = itemPostDto.ItemName,
                AddDate = new DateTime().Date,
                RenewDate = new DateTime().Date,
            };

            _chickenContext.ItemTables.Add(itemTable);

            await _chickenContext.SaveChangesAsync();

            return itemTable;
        }

        // 修改
        public async Task<ResponseMessage> 修改品項資料(ItemPutDto itemPutDto)
        {

            var itemTable = await _chickenContext.ItemTables.SingleOrDefaultAsync(a => a.ItemId == itemPutDto.ItemId);

            if (itemTable == null)
            {
                return new ResponseMessage("找不到對應的品項", 404);
            }

            itemTable.RenewDate = new DateTime().Date;

            _chickenContext.ItemTables.Update(itemTable).CurrentValues.SetValues(itemPutDto);

            await _chickenContext.SaveChangesAsync();

            return new ResponseMessage("修改成功", 200, itemTable);
        }

        // 過濾
        private IQueryable<ItemTable> 過濾ItemGetDto資料(IQueryable<ItemTable> itemTable, ItemGetDto itemGetDto)
        {
            if (itemGetDto.TableId.HasValue && itemGetDto.TableId.Value > 0)
            {
                itemTable = itemTable.Where(a => a.TableId == itemGetDto.TableId);
            }

            if (itemGetDto.ItemId.HasValue)
            {
                itemTable = itemTable.Where(a => a.ItemId == itemGetDto.ItemId.Value);
            }

            if (!string.IsNullOrEmpty(itemGetDto.ItemName))
            {
                itemTable = itemTable.Where(a => a.ItemName == itemGetDto.ItemName);
            }

            if (itemGetDto.AddDate.HasValue && itemGetDto.AddDate > DateTime.Now)
            {
                itemTable = itemTable.Where(a => a.AddDate == itemGetDto.AddDate);
            }

            if (itemGetDto.RenewDate.HasValue && itemGetDto.RenewDate > DateTime.Now)
            {
                itemTable = itemTable.Where(a => a.RenewDate == itemGetDto.RenewDate);
            }

            return itemTable;
        }
    }
}
