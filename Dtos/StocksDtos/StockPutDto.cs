using ChickenApplication.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.Dtos.StocksDtos
{
    public class StockPutDto : StockEditAbstracts
    {
        public string? ItemName { get; set; }
    }
}
