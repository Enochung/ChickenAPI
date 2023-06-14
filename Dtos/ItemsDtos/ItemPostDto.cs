using ChickenApplication.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.ItemsDtos
{
    public class ItemPostDto : ItemEditAbstracts
    {
        //[Required(ErrorMessage = "ItemId is required")]
        //[RegularExpression(@"^\b[A-Fa-f\d]{8}(-[A-Fa-f\d]{4}){3}-[A-Fa-f\d]{12}\b$", ErrorMessage = "ItemId must be a valid GUID")]
        //public Guid ItemId { get; set; }

        //[Required(ErrorMessage = "ItemName is required")]
        //[RegularExpression(@"\S", ErrorMessage = "ItemName cannot be empty or contain only whitespace")]
        //public string ItemName { get; set; } = null!;
    }
}
