using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;

            return date > DateTime.Now;
        }
    }
}
