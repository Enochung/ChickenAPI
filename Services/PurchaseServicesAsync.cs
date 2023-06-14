using ChickenApplication.Dtos.PurchasesDtos;
using ChickenApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace ChickenApplication.Services
{
    public class PurchaseServicesAsync
    {
        private readonly ChickenContext _chickenContext;

        public PurchaseServicesAsync(ChickenContext chickenContext)
        {
            _chickenContext = chickenContext;
        }

        // 查詢
        public async Task<ResponseMessage> 取得所有進貨資料()
        {
            var result = await _chickenContext.PurchaseTables
                .Join(_chickenContext.ItemTables,
                purchaseTable => purchaseTable.ItemId,
                itemTable => itemTable.ItemId,
                (purchaseTable, itemTable) => new
                {
                    purchaseTable,
                    itemTable
                })
                .Join(_chickenContext.SupplierTables,
                joinResult => joinResult.purchaseTable.SupplierId,
                supplierTable => supplierTable.SupplierId,
                (joinResult, supplierTable) => new PurchaseTableWithItemNameAndSupplierName
                {
                    TableId = joinResult.purchaseTable.TableId,
                    ItemName = joinResult.itemTable.ItemName,
                    PurchaseDate = joinResult.purchaseTable.PurchaseDate,
                    PurchaseQuantity = joinResult.purchaseTable.PurchaseQuantity,
                    PurchasePrice = joinResult.purchaseTable.PurchasePrice,
                    ItemExp = joinResult.purchaseTable.ItemExp,
                    ItemLot = joinResult.purchaseTable.ItemLot,
                    SupplierName = supplierTable.SupplierName,
                    AddDate = joinResult.purchaseTable.AddDate,
                    RenewDate = joinResult.purchaseTable.RenewDate,
                }).ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);

        }

        // 有條件查詢
        public async Task<ResponseMessage> 取得條件進貨資料(PurchaseGetDto purchaseGetDto)
        {
            var purchaseTable = _chickenContext.PurchaseTables.AsQueryable();

            purchaseTable = 過濾PurchaseGetDto資料(purchaseTable, purchaseGetDto);

            var result = await purchaseTable
                .Join(_chickenContext.ItemTables,
                purchase => purchase.ItemId,
                item => item.ItemId,
                (purchase, item) => new { Purchase = purchase, Item = item })
                .Join(_chickenContext.SupplierTables,
                joinResult => joinResult.Purchase.SupplierId,
                supplier => supplier.SupplierId,
                (joinResult, supplier) => new PurchaseTableWithItemNameAndSupplierName
                {
                    TableId = joinResult.Purchase.TableId,
                    ItemName = joinResult.Item.ItemName,
                    PurchaseDate = joinResult.Purchase.PurchaseDate,
                    PurchaseQuantity = joinResult.Purchase.PurchaseQuantity,
                    PurchasePrice = joinResult.Purchase.PurchasePrice,
                    ItemExp = joinResult.Purchase.ItemExp,
                    ItemLot = joinResult.Purchase.ItemLot,
                    SupplierName = supplier.SupplierName,
                    AddDate = joinResult.Purchase.AddDate,
                    RenewDate = joinResult.Purchase.RenewDate
                })
                .ToListAsync();

            if (!result.Any())
            {
                return new ResponseMessage("查詢失敗", 404);
            }

            return new ResponseMessage("查詢成功", 200, result);
        }

        // 新增
        public async Task<ResponseMessage> 新增進貨資料(PurchasePostDto purchasePostDto)
        {
            var item = await _chickenContext.ItemTables.SingleOrDefaultAsync(a => a.ItemName == purchasePostDto.ItemName);

            if (item == null)
            {
                return new ResponseMessage("找不到對應的品項", 404);
            }

            var supplier = await _chickenContext.SupplierTables.SingleOrDefaultAsync(a => a.SupplierName == purchasePostDto.SupplierName);

            if (supplier == null)
            {
                return new ResponseMessage("找不到對應的供應商", 404);
            }

            var purchaseTable = new PurchaseTable
            {
                ItemId = item.ItemId,
                PurchaseDate = purchasePostDto.PurchaseDate,
                PurchaseQuantity = purchasePostDto.PurchaseQuantity,
                PurchasePrice = purchasePostDto.PurchasePrice,
                ItemExp = purchasePostDto.ItemExp,
                ItemLot = purchasePostDto.ItemLot,
                SupplierId = supplier.SupplierId,
                AddDate = new DateTime().Date,
                RenewDate = new DateTime().Date,
            };

            _chickenContext.PurchaseTables.Add(purchaseTable);

            await 新增進貨資料更新庫存資料(item.ItemId, purchasePostDto.PurchaseQuantity);

            await _chickenContext.SaveChangesAsync();

            return new ResponseMessage("新增成功", 200, purchaseTable);
        }

        // 修改
        // 設置一個變數用以取代
        public async Task<ResponseMessage> 修改進貨資料(PurchasePutDto purchasePutDto)
        {
            if (!string.IsNullOrWhiteSpace(purchasePutDto.ItemName))
            {
                // 確認ItemName對應的ItemId
                var item = await _chickenContext.ItemTables.SingleOrDefaultAsync(a => a.ItemName == purchasePutDto.ItemName);

                if (item == null)
                {
                    return new ResponseMessage("找不到對應的品項", 404);
                }

                purchasePutDto.ItemId = item.ItemId;

                // 確認ItemId是否已有庫存資料
                var checkStockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == purchasePutDto.ItemId);

                if (checkStockTable == null)
                {
                    return new ResponseMessage("要修改的品項名稱不存在庫存資料", 404);
                }
            }

            // 確認TableId存在於進貨資料中
            var purchaseTable = await _chickenContext.PurchaseTables.SingleOrDefaultAsync(a => a.TableId == purchasePutDto.TableId);

            if (purchaseTable == null)
            {
                return new ResponseMessage("編號輸入有誤", 404);
            }

            //只更改ItemId => 舊ItemId的庫存數量要減少，新ItemId的庫存數量要增加 OK
            //同時更改ItemId與PurchaseQuantity => 舊ItemId的庫存數量要減少，新ItemId的庫存數量要增加
            if (purchasePutDto.ItemId.HasValue && purchasePutDto.ItemId.Value != purchaseTable.ItemId)
            {
                await ItemId不同時更新庫存資料(purchaseTable, purchasePutDto);
            }

            //只更改PurchaseQuantity => 新的數量大於舊的數量ItemId的庫存數量要增加，新的數量小於舊的數量ItemId的庫存數量要減少
            if (!purchasePutDto.ItemId.HasValue && purchasePutDto.PurchaseQuantity.HasValue && purchasePutDto.PurchaseQuantity.Value != purchaseTable.PurchaseQuantity)
            {
                await PurchaseQuantity不同時更新庫存資料(purchaseTable, purchasePutDto);
            }

            purchaseTable = await 過濾PurchasePutDto資料(purchaseTable, purchasePutDto);

            purchaseTable.RenewDate = DateTime.Now;

            _chickenContext.PurchaseTables.Update(purchaseTable);

            await _chickenContext.SaveChangesAsync();

            return new ResponseMessage("進貨資料修改成功", 200, purchaseTable);
        }


        // 過濾
        private IQueryable<PurchaseTable> 過濾PurchaseGetDto資料(IQueryable<PurchaseTable> purchaseTable, PurchaseGetDto purchaseGetDto)
        {
            if (purchaseGetDto.TableId.HasValue && purchaseGetDto.TableId.Value > 0)
            {
                purchaseTable = purchaseTable.Where(a => a.TableId == purchaseGetDto.TableId);
            }

            if (!string.IsNullOrEmpty(purchaseGetDto.ItemName))
            {
                var itemIds = _chickenContext.ItemTables
                    .Where(item => item.ItemName == purchaseGetDto.ItemName)
                    .Select(item => item.ItemId)
                    .ToList();

                purchaseTable = purchaseTable.Where(a => itemIds.Contains(a.ItemId));
            }

            if (purchaseGetDto.PurchaseDate.HasValue)
            {
                purchaseTable = purchaseTable.Where(a => a.PurchaseDate == purchaseGetDto.PurchaseDate);
            }

            if (purchaseGetDto.PurchaseQuantity.HasValue && purchaseGetDto.PurchaseQuantity.Value > 0)
            {
                purchaseTable = purchaseTable.Where(a => a.PurchaseQuantity == purchaseGetDto.PurchaseQuantity);
            }

            if (purchaseGetDto.PurchasePrice.HasValue && purchaseGetDto.PurchasePrice.Value > 0)
            {
                purchaseTable = purchaseTable.Where(a => a.PurchasePrice == purchaseGetDto.PurchasePrice);
            }

            if (purchaseGetDto.ItemExp.HasValue)
            {
                purchaseTable = purchaseTable.Where(a => a.ItemExp == purchaseGetDto.ItemExp);
            }

            if (!string.IsNullOrEmpty(purchaseGetDto.ItemLot))
            {
                purchaseTable = purchaseTable.Where(a => a.ItemLot == purchaseGetDto.ItemLot);
            }

            if (!string.IsNullOrEmpty(purchaseGetDto.SupplierName))
            {
                var supplierIds = _chickenContext.SupplierTables
                    .Where(supplier => supplier.SupplierName == purchaseGetDto.SupplierName)
                    .Select(supplier => supplier.SupplierId)
                    .ToList();

                purchaseTable = purchaseTable.Where(a => supplierIds.Contains(a.SupplierId));
            }

            if (purchaseGetDto.AddDate.HasValue)
            {
                purchaseTable = purchaseTable.Where(a => a.AddDate == purchaseGetDto.AddDate);
            }

            if (purchaseGetDto.RenewDate.HasValue)
            {
                purchaseTable = purchaseTable.Where(a => a.RenewDate == purchaseGetDto.RenewDate);
            }

            return purchaseTable;
        }

        private async Task<PurchaseTable> 過濾PurchasePutDto資料(PurchaseTable purchaseTable, PurchasePutDto purchasePutDto)
        {
            if (purchasePutDto.ItemId.HasValue)
            {
                purchaseTable.ItemId = purchasePutDto.ItemId.Value;
            }

            if (purchasePutDto.PurchaseDate.HasValue)
            {
                purchaseTable.PurchaseDate = purchasePutDto.PurchaseDate.Value;
            }

            if (purchasePutDto.PurchaseQuantity.HasValue && purchasePutDto.PurchaseQuantity.Value > 0)
            {
                purchaseTable.PurchaseQuantity = purchasePutDto.PurchaseQuantity.Value;
            }

            if (purchasePutDto.PurchasePrice.HasValue && purchasePutDto.PurchasePrice.Value > 0)
            {
                purchaseTable.PurchasePrice = purchasePutDto.PurchasePrice.Value;
            }

            if (purchasePutDto.ItemExp.HasValue)
            {
                purchaseTable.ItemExp = purchasePutDto.ItemExp.Value;
            }

            if (!string.IsNullOrEmpty(purchasePutDto.ItemLot))
            {
                purchaseTable.ItemLot = purchasePutDto.ItemLot;
            }

            if (!string.IsNullOrEmpty(purchasePutDto.SupplierName))
            {
                var supplier = await _chickenContext.SupplierTables.SingleOrDefaultAsync(a => a.SupplierName == purchasePutDto.SupplierName);

                if (supplier != null)
                {
                    purchaseTable.SupplierId = supplier.SupplierId;
                }
            }

            return purchaseTable;
        }

        // 更新
        private async Task 新增進貨資料更新庫存資料(Guid _ItemId, int _ItemStock)
        {
            var stockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == _ItemId);

            if (stockTable == null)
            {
                var addStockTable = new StockTable
                {
                    ItemId = _ItemId,
                    ItemStock = _ItemStock,
                    AddDate = DateTime.Now,
                    RenewDate = DateTime.Now,
                };

                _chickenContext.StockTables.Add(addStockTable);
            }
            else
            {
                stockTable.ItemStock += _ItemStock;
                stockTable.RenewDate = DateTime.Now;

                _chickenContext.StockTables.Update(stockTable).CurrentValues.SetValues(stockTable);
            }
        }

        private async Task<ResponseMessage> ItemId不同時更新庫存資料(PurchaseTable purchaseTable, PurchasePutDto purchasePutDto)
        {
            // 舊庫存
            var oldStockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == purchaseTable.ItemId);

            if (oldStockTable == null)
            {
                return new ResponseMessage("找不到舊的庫存資料", 404);
            }

            oldStockTable.ItemStock -= purchaseTable.PurchaseQuantity;

            oldStockTable.RenewDate = DateTime.Now;

            _chickenContext.StockTables.Update(oldStockTable);

            // 新庫存
            var newStockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == purchasePutDto.ItemId);

            if (newStockTable == null)
            {
                return new ResponseMessage("找不到新的庫存資料", 404);
            }

            // purchasePutDto.PurchaseQuantity是否有做更動
            if (purchasePutDto.PurchaseQuantity == null)
            {
                newStockTable.ItemStock += purchaseTable.PurchaseQuantity;
            }
            else
            {
                newStockTable.ItemStock += (int)purchasePutDto.PurchaseQuantity;
            }

            newStockTable.RenewDate = DateTime.Now;

            _chickenContext.StockTables.Update(newStockTable);

            return new ResponseMessage("庫存資料修改成功", 200);
        }

        private async Task PurchaseQuantity不同時更新庫存資料(PurchaseTable purchaseTable, PurchasePutDto purchasePutDto)
        {
            var stockTable = await _chickenContext.StockTables.SingleOrDefaultAsync(a => a.ItemId == purchaseTable.ItemId);

            if (stockTable != null)
            {
                stockTable.ItemStock += (purchasePutDto.PurchaseQuantity.Value - purchaseTable.PurchaseQuantity);

                stockTable.RenewDate = DateTime.Now;

                _chickenContext.StockTables.Update(stockTable);
            }
        }
    }
}
