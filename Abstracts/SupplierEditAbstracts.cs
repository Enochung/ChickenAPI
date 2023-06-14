using ChickenApplication.Dtos.ItemsDtos;
using ChickenApplication.Dtos.SuppliersDtos;
using ChickenApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Abstracts
{
    public class SupplierEditAbstracts
    {

        [Required(ErrorMessage = "Supplier name is required.")]
        [RegularExpression(".+", ErrorMessage = "Item lot is required.")]
        [DataType(DataType.Text)]
        public string SupplierName { get; set; } = null!;

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Supplier phone must be a numeric value.")]
        public string? SupplierPhone { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Supplier phone must be a numeric value.")]
        public string? SupplierAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ChickenContext chickenContext = (ChickenContext)validationContext.GetService(typeof(ChickenContext));

            var finds = chickenContext.SupplierTables.Where(a => a.SupplierName == SupplierName);
            //取得對象的實例

            if (this.GetType() == typeof(SupplierPutDto))
            {
                var dtoUpdate = (SupplierPutDto)this;
                finds = finds.Where(a => a.SupplierId != dtoUpdate.SupplierId);
            }

            if (finds.FirstOrDefault() != null)
            {
                yield return new ValidationResult("已存在相同產品名稱", new string[] { "ItemName" });
            }
        }
    }
}
