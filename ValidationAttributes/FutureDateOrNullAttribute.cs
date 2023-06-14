using System.ComponentModel.DataAnnotations;

namespace ChickenApplication.ValidationAttributes
{
    public class FutureDateOrNullAttribute : ValidationAttribute
    {
        public FutureDateOrNullAttribute()
        {
            ErrorMessage = "Purchase date must be a future date or null.";
        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var date = (DateTime)value;

            return date > DateTime.Now;
        }
    }
}
