using ChickenApplication.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.PurchasesDtos
{
    public class PurchasePostDto
    {
        [RegularExpression(".+", ErrorMessage = "Itemlot is required.")]
        [DataType(DataType.Text)]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "PurchaseDate is required")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "PurchaseQuantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Purchase Quantity must be a positive number.")]
        public int PurchaseQuantity { get; set; }

        [Required(ErrorMessage = "PurchasePrice is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Purchase price must be a positive number.")]
        public int PurchasePrice { get; set; }

        [Required(ErrorMessage = "ItemExp is required")]
        [FutureDate]
        public DateTime ItemExp { get; set; }

        [RegularExpression(".+", ErrorMessage = "Itemlot is required.")]
        [DataType(DataType.Text)]
        public string? ItemLot { get; set; } = null!;

        [RegularExpression(".+", ErrorMessage = "Itemlot is required.")]
        [DataType(DataType.Text)]
        public string SupplierName { get; set; }
    }
}
