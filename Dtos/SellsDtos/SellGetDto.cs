namespace ChickenApplication.Dtos.SellsDtos
{
    public class SellGetDto
    {
        public int? TableId { get; set; }

        public Guid? ItemId { get; set; }

        public int? SellQuantity { get; set; }

        public int? SellPrice { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? RenewDate { get; set; }
    }
}
