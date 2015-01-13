using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Converters
{
    /// <summary>
    /// Klasa odpowiedzialna za wypisywanie informacji o zawartości zamówienia
    /// </summary>
    public class OrderContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListViewItem item = (ListViewItem)value;
            Order order = (Order)(item.Content);

            StringBuilder sb = new StringBuilder();
            foreach(var menuItem in order.MenuItems)
            {
                sb.Append("(" + menuItem.Quantity + ")" + menuItem.MenuItem.Name + ", ");
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
