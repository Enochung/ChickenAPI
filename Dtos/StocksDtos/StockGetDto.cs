namespace ChickenApplication.Dtos.StocksDtos
{
    public class StockGetDto
    {
        public int? TableId { get; set; }

        public Guid? ItemId { get; set; }

        public int? ItmeStock { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? RenewDate { get; set; }

    }
}
