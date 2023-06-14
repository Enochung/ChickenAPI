using ChickenApplication.Dtos.ItemsDtos;
using ChickenApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Abstracts
{
    public class ItemEditAbstracts : IValidatableObject
    {
        [Required(ErrorMessage = "ItemName is required")]
        //[RegularExpression(@"\S", ErrorMessage = "ItemName cannot be empty or contain only whitespace")]
        public string ItemName { get; set; } = null!;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ChickenContext chickenContext = (ChickenContext)validationContext.GetService(typeof(ChickenContext));

            var finds = chickenContext.ItemTables.Where(a => a.ItemName == ItemName);
            //取得對象的實例

            if (this.GetType() == typeof(ItemPutDto))
            {
                var dtoUpdate = (ItemPutDto)this;
                finds = finds.Where(a => a.ItemId != dtoUpdate.ItemId);
            }

            if (finds.FirstOrDefault() != null)
            {
                yield return new ValidationResult("已存在相同產品名稱", new string[] { "ItemName" });
            }
        }
    }
}
