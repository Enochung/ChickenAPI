namespace ChickenApplication.Dtos.SellsDtos
{
    public class SellTableWithItemName
    {
        public int TableId { get; set; }

        public string ItemName { get; set; }

        public int SellQuantity { get; set; }

        public int SellPrice { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime RenewDate { get; set; }
    }
}
