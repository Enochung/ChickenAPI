using ChickenApplication.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.ItemsDtos
{
    public class ItemPutDto : ItemEditAbstracts
    {
        //[Required(ErrorMessage = "ItemName is required")]
        //[RegularExpression(@"\S", ErrorMessage = "ItemName cannot be empty or contain only whitespace")]
        //public string ItemName { get; set; } = null!;

        [Required(ErrorMessage = "ItemName is required")]
        public Guid ItemId { get; set; }

    }
}
