using ClassLib.DataStructures;
using System;
using System.Collections.Generic;
namespace WebUI.Models
{
    public class MenuListViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public String CurrentCategory { get; set; }
    }
}