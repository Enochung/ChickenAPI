using ChickenApplication.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.PurchasesDtos
{
    public class PurchasePutDto
    {
        public int TableId { get; set; }

        public Guid? ItemId { get; set; }

        public string? ItemName { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public int? PurchaseQuantity { get; set; }

        public int? PurchasePrice { get; set; }

        public DateTime? ItemExp { get; set; }

        public string? ItemLot { get; set; } = null!;

        public string? SupplierName { get; set; }
    }
}
