namespace ChickenApplication.Dtos.SellsDtos
{
    public class SellPutDto
    {
        public int TableId { get; set; }

        public Guid? ItemId { get; set; }

        public int? SellQuantity { get; set; }

        public int? SellPrice { get; set; }
    }
}
