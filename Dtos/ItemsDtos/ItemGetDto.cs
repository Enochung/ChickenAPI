using ChickenApplication.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.ItemsDtos
{
    public class ItemGetDto
    {
        public int? TableId { get; set; }

        public Guid? ItemId { get; set; }

        public string? ItemName { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? RenewDate { get; set; }
    }
}
