using ClassLib.DbDataStructures;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickDatabaseSetup
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("### Obtaining Manager Data Access...");
            IManagerDataAccess managerDataAccess = new DataAccessClass();
            IDataWipe dataWipe = new DataAccessClass();
            Console.WriteLine("### Done.");
            Console.Write("### Cleaning any existing data...");
            WipeAllDatabaseData(dataWipe, managerDataAccess);
            Console.WriteLine("### Done.");
            Console.WriteLine("### Press any key to fill database with data...");
            var keyInfo = Console.ReadKey();
            DatabaseDataFill(dataWipe, managerDataAccess);
            Console.WriteLine("### Database filled with data. Press any key to wipe data...");
            keyInfo = Console.ReadKey();
            WipeAllDatabaseData(dataWipe, managerDataAccess);
            Console.WriteLine("### Done. Press any key to exit.");
            keyInfo = Console.ReadKey();
            Console.WriteLine("### Exiting.");
        }

        private static void DatabaseDataFill(IDataWipe dataWipe, IManagerDataAccess managerDataAccess)
        {
            try
            {
                string catName1 = "Napoje wysokoprocentowe";
                string catDescription1 = "Kiedy piwo po prostu nie wystarcza.";
                Console.Write("=> Adding MenuItemCategory Name = {0}, Description = {1}...", catName1, catDescription1);
                MenuItemCategory cat1 = managerDataAccess.AddMenuItemCategory(catName1, catDescription1);
                Console.WriteLine("Done.");

                string catName2 = "Napoje niskoprocentowe";
                string catDescription2 = "Każda okazja jest dobra.";
                Console.Write("=> Adding MenuItemCategory Name = {0}, Description = {1}...", catName2, catDescription2);
                MenuItemCategory cat2 = managerDataAccess.AddMenuItemCategory(catName2, catDescription2);
                Console.WriteLine("Done.");

                string catName3 = "Napoje bezalkoholowe";
                string catDescription3 = "Przegrałeś zakład albo musisz prowadzić.";
                Console.Write("=> Adding MenuItemCategory Name = {0}, Description = {1}...", catName3, catDescription3);
                MenuItemCategory cat3 = managerDataAccess.AddMenuItemCategory(catName3, catDescription3);
                Console.WriteLine("Done.");

                string menuItemName1 = "DeLuxe 55%";
                string menuItemDescription1 = "Wykwintny destylat ziem wschodnich.";
                Money menuItemPrice1 = new Money() { Amount = 25f, Currency = "PLN" };
                Console.Write("=> Adding MenuItem Name = {0}, Description = {1}, Category = {2}, Price = {3} {4}...", menuItemName1, menuItemDescription1, catName1, menuItemPrice1.Amount, menuItemPrice1.Currency);
                MenuItem menuItem1 = managerDataAccess.AddMenuItem(menuItemName1, menuItemDescription1, cat1.Id, menuItemPrice1);
                Console.WriteLine("Done.");

                string menuItemName2 = "Ciociosan";
                string menuItemDescription2 = "Ni to piwo, ni to wino...";
                Money menuItemPrice2 = new Money() { Amount = 8f, Currency = "PLN" };
                Console.Write("=> Adding MenuItem Name = {0}, Description = {1}, Category = {2}, Price = {3} {4}...", menuItemName2, menuItemDescription2, catName2, menuItemPrice2.Amount, menuItemPrice2.Currency);
                MenuItem menuItem2 = managerDataAccess.AddMenuItem(menuItemName2, menuItemDescription2, cat2.Id, menuItemPrice2);
                Console.WriteLine("Done.");

                string menuItemName3 = "Napój o smaku pomarańczowopodobnym";
                string menuItemDescription3 = "Szczerze, nie polecamy ...";
                Money menuItemPrice3 = new Money() { Amount = 2f, Currency = "PLN" };
                Console.Write("=> Adding MenuItem Name = {0}, Description = {1}, Category = {2}, Price = {3} {4}...", menuItemName3, menuItemDescription3, catName3, menuItemPrice3.Amount, menuItemPrice3.Currency);
                MenuItem menuItem3 = managerDataAccess.AddMenuItem(menuItemName3, menuItemDescription3, cat3.Id, menuItemPrice3);
                Console.WriteLine("Done.");

                int tableNumber1 = 1;
                string tableDescription1 = "Ten ładny od razu przy wyjściu";
                Console.Write("=> Adding Table Number {0}, Description = {1}...", tableNumber1, tableDescription1);
                Table table1 = managerDataAccess.AddTable(tableNumber1, tableDescription1);
                Console.WriteLine("Done.");

                int tableNumber2 = 2;
                string tableDescription2 = "Ten mniej ładny stojący na środku pomieszczenia";
                Console.Write("=> Adding Table Number {0}, Description = {1}...", tableNumber2, tableDescription2);
                Table table2 = managerDataAccess.AddTable(tableNumber2, tableDescription2);
                Console.WriteLine("Done.");

                int tableNumber3 = 3;
                string tableDescription3 = "Ta ledwo trzymająca się kupy sterta desek stojąca przy toaletach.";
                Console.Write("=> Adding Table Number {0}, Description = {1}...", tableNumber3, tableDescription3);
                Table table3 = managerDataAccess.AddTable(tableNumber3, tableDescription3);
                Console.WriteLine("Done.");

                string waiter1FirstName = "Iron";
                string waiter1LastName = "Man";
                string waiter1Login = "ironman";
                string waiter1Password = "tardis";
                Console.Write("=> Adding Waiter First name = {0}, Last name = {1}, Login = {2}, Password = {3}...", waiter1FirstName, waiter1LastName, waiter1Login, waiter1Password);
                WaiterContext waiter1 = managerDataAccess.AddWaiter(waiter1FirstName, waiter1LastName, waiter1Login, waiter1Password);
                Console.WriteLine("Done.");

                string waiter2FirstName = "Captain";
                string waiter2LastName = "America";
                string waiter2Login = "captainamerica";
                string waiter2Password = "shield";
                Console.Write("=> Adding Waiter First name = {0}, Last name = {1}, Login = {2}, Password = {3}...", waiter2FirstName, waiter2LastName, waiter2Login, waiter2Password);
                WaiterContext waiter2 = managerDataAccess.AddWaiter(waiter2FirstName, waiter2LastName, waiter2Login, waiter2Password);
                Console.WriteLine("Done.");

                string waiter3FirstName = "Jan";
                string waiter3LastName = "Zagłoba";
                string waiter3Login = "zagloba";
                string waiter3Password = "zacnymiod";
                Console.Write("=> Adding Waiter First name = {0}, Last name = {1}, Login = {2}, Password = {3}...", waiter3FirstName, waiter3LastName, waiter3Login, waiter3Password);
                WaiterContext waiter3 = managerDataAccess.AddWaiter(waiter3FirstName, waiter3LastName, waiter3Login, waiter3Password);
                Console.WriteLine("Done.");
            }
            catch(Exception e)
            {
                Console.Write(" *** An error Occured. Wiping database...");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException != null ? e.InnerException.Message : String.Empty);
                WipeAllDatabaseData(dataWipe, managerDataAccess);
                Console.WriteLine("Done.");
            }
        }

        private static void WipeAllDatabaseData(IDataWipe dataWipe, IManagerDataAccess managerDataAccess)
        {
            try
            {
                IEnumerable<Order> orders = managerDataAccess.GetOrders();
                foreach (Order order in orders)
                {
                    Console.Write("=> Erasing Order Id = {0}...", order.Id);
                    if (dataWipe.WipeOrder(order.Id))
                        Console.WriteLine("Done.");
                    else
                        Console.WriteLine(" *** Error !!!");
                }

                IEnumerable<MenuItem> menuItems = managerDataAccess.GetMenuItems();
                foreach (MenuItem menuItem in menuItems)
                {
                    Console.Write("=> Erasing MenuItem Id = {0}...", menuItem.Id);
                    if (dataWipe.WipeMenuItem(menuItem.Id))
                        Console.WriteLine("Done.");
                    else
                        Console.WriteLine(" *** Error !!!");
                }

                IEnumerable<MenuItemCategory> menuItemCategories = managerDataAccess.GetMenuItemCategories();
                foreach (MenuItemCategory menuItemCategory in menuItemCategories)
                {
                    Console.Write("=> Erasing MenuItemCategory Id = {0}...", menuItemCategory.Id);
                    if (dataWipe.WipeMenuItemCategory(menuItemCategory.Id))
                        Console.WriteLine("Done.");
                    else
                        Console.WriteLine(" *** Error !!!");
                }

                IEnumerable<Table> tables = managerDataAccess.GetTables();
                foreach (Table table in tables)
                {
                    Console.Write("=>  Erasing Table Id = {0}...", table.Id);
                    if (dataWipe.WipeTable(table.Id))
                        Console.WriteLine("Done.");
                    else
                        Console.WriteLine(" *** Error !!!");
                }

                IEnumerable<WaiterContext> waiters = managerDataAccess.GetWaiters();
                foreach (WaiterContext waiterContext in waiters)
                {
                    Console.Write("=> Erasing Waiter Id = {0}... ", waiterContext.Id);
                    if (dataWipe.WipeWaiter(waiterContext.Id))
                        Console.WriteLine("Done.");
                    else
                        Console.WriteLine(" **** Error !!!");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(" *** Error erasing data...");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException != null ? e.InnerException.Message : String.Empty);
            }
        }
    }
}
