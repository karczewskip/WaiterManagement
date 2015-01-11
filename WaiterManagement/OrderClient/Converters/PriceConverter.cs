using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace OrderClient.Converters
{
    /// <summary>
    /// Klasa odpowiedzialna za wypisywanie ceny zamówienia
    /// </summary>
    public class PriceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListViewItem item = (ListViewItem)value;
            Order order = (Order)(item.Content);

            double price = 0;
            foreach (var menuItem in order.MenuItems)
            {
                price += menuItem.MenuItem.Price.Amount * menuItem.Quantity;
            }

            return Math.Round( price,2).ToString() + " PLN";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
