using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ClassLib.DataStructures
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute()
            : base(
                typeof (DateTime), DateTime.Now.AddDays(1).ToShortDateString(),
                DateTime.Now.AddMonths(2).ToShortDateString())
        {
            
        }
    }

    public class OrderDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        [DateRange(ErrorMessage = "Wrong Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a number")]
        public long Number { get; set; }
    }
}