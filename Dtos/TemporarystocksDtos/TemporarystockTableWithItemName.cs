namespace ChickenApplication.Dtos.PurchasesDtos
{
    public class TemporarystockTableWithItemName
    {
        public int TableId { get; set; }

        public string ItemName { get; set; }

        public int ItemTemporarystock { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime RenewDate { get; set; }
    }
}
