using System;
using System.ComponentModel.DataAnnotations;

namespace ClassLib.DataStructures
{
    public class DateRangeAttribute : ValidationAttribute
    {
        public static DateTime StartDate { get { return DateTime.Today.AddDays(1).AddHours(9); } }
        static public DateTime EndDate { get { return DateTime.Today.AddMonths(1).AddHours(21); } }

        public DateRangeAttribute()
        {
            ErrorMessage = "Date should be between: " + StartDate.ToShortDateString() + " - " + EndDate.ToShortDateString() + "\nBetween 9-21";
        }

        public override bool IsValid(object value)
        {
            var currentDate = (DateTime) value;
            if (StartDate > currentDate || currentDate > EndDate)
                return false;

            return currentDate.Hour >= 9 && currentDate.Hour < 21;
        }
    }

    public class OrderDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        [DateRange]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a number")]
        public long Number { get; set; }
    }
}