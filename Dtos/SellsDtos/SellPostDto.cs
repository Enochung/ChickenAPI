using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.SellsDtos
{
    public class SellPostDto
    {
        [Required(ErrorMessage = "ItemId is required")]
        [RegularExpression(@"^\b[A-Fa-f\d]{8}(-[A-Fa-f\d]{4}){3}-[A-Fa-f\d]{12}\b$", ErrorMessage = "ItemId must be a valid GUID")]
        public Guid ItemId { get; set; }

        [Required(ErrorMessage = "SellQuantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Sell Quantity must be a positive number.")]
        public int SellQuantity { get; set; }

        [Required(ErrorMessage = "SellPrice is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Sell Price must be a positive number.")]
        public int SellPrice { get; set; }
    }
}
