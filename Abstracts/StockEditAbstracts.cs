using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Abstracts
{
    public class StockEditAbstracts
    {
        [Required(ErrorMessage = "ItemId is required")]
        [RegularExpression(@"^\b[A-Fa-f\d]{8}(-[A-Fa-f\d]{4}){3}-[A-Fa-f\d]{12}\b$", ErrorMessage = "ItemId must be a valid GUID")]
        public Guid ItemId { get; set; }

        [Required(ErrorMessage = "ItemName is required")]
        public int ItmeStock { get; set; }
    }
}
