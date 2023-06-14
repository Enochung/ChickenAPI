using ChickenApplication.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.SuppliersDtos
{
    public class SupplierPutDto : SupplierEditAbstracts
    {
        [Required(ErrorMessage = "ItemName is required")]
        public Guid SupplierId { get; set; }
    }
}
