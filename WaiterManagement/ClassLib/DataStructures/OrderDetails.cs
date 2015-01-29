using System;
using System.ComponentModel.DataAnnotations;

namespace ClassLib.DataStructures
{
    public class OrderDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a number")]
        public string Number { get; set; }
    }
}