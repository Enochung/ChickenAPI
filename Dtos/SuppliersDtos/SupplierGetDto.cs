namespace ChickenApplication.Dtos.SuppliersDtos
{
    public class SupplierGetDto
    {
        public int? TableId { get; set; }

        public Guid? SupplierId { get; set; }

        public string? SupplierName { get; set; } = null!;

        public string? SupplierPhone { get; set; }

        public string? SupplierAddress { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? RenewDate { get; set; }
    }
}
