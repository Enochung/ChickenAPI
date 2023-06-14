namespace ChickenApplication.Dtos.StocksDtos
{
    public class stockTableWithItemName
    {
        public int TableId { get; set; }

        public string ItemName { get; set; }

        public int ItemStock { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime RenewDate { get; set; }
    }
}
