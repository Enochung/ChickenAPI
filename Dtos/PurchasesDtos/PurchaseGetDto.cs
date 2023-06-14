namespace ChickenApplication.Dtos.PurchasesDtos
{
    public class PurchaseGetDto
    {
        public int? TableId { get; set; }

        public string? ItemName { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public int? PurchaseQuantity { get; set; }

        public int? PurchasePrice { get; set; }

        public DateTime? ItemExp { get; set; }

        public string? ItemLot { get; set; }

        public string? SupplierName { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? RenewDate { get; set; }
    }

}
