using System.Collections.Generic;
using System.Linq;

namespace ClassLib.DataStructures
{
    public class Cart
    {
        private readonly List<CartLine> lineCollection = new List<CartLine>();

        public string Currency
        {
            get { return ApplicationResources.DefaultCurrency; }
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        public void AddItem(MenuItem menuItem, int quantity)
        {
            var line = lineCollection.FirstOrDefault(p => p.MenuItem.Id == menuItem.Id);
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    MenuItem = menuItem,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(MenuItem menuItem)
        {
            lineCollection.RemoveAll(l => l.MenuItem.Id == menuItem.Id);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => (decimal) (e.MenuItem.Price.Amount*e.Quantity));
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }

    public class CartLine
    {
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
    }
}