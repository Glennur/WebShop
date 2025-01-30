using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class AdminPage
    {
        public static void AdminMenu()
        {
            List<string> adminMenu = new List<string>();
            using (var db = new Models.WebShopContext())
            {
                var categories = db.Categories.ToList();
                
                adminMenu.Add("[L] Logga ut");
                
                adminMenu.Add("[O] Ordrar");
                adminMenu.Add("[S] Sök på vara");
                adminMenu.Add("[C] Skapa ny vara");
                adminMenu.Add("[Q] Avsluta");
            }
            var windowTop = new Window("", 0, 10, adminMenu);
            windowTop.Draw();

        }
        public static void ChangeStartPage(ConsoleKeyInfo key)
        {

            Console.SetCursorPosition(30, 16);
            Console.Write($"Vilken vara vill du lägga på plats {key.Key}?: ");
            
            using (var myDb = new WebShopContext())
            {

                var result = myDb.Products.ToList();
                List<string> allProducts = new List<string>();
                if (result.Count == 0)
                {
                    allProducts.Add("Inga produkter hittades".PadRight(66));


                }
                else
                {
                    foreach (var product in result)
                    {
                        allProducts.Add($"{product.Id,3}. {product.Name.PadRight(53)}{product.UnitsPrice:F2} kr");
                    }


                }

                var AllProducts1 = new Window("Alla varor i sortimentet", 30, 18, allProducts);
                AllProducts1.Draw();
                bool changeStart = false;
                do
                {
                    Console.SetCursorPosition(69, 16);
                    string? input = Console.ReadLine();
                    bool changeStartPage = int.TryParse(input, out int value);
                    if(!changeStartPage)
                    {
                        Console.WriteLine("Felaktig inmatning!");
                    }
                    else if (allProducts.Count < value)
                    {
                        Console.WriteLine("Varan hittades inte!");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.X)
                        {
                            Constants.startPageX = value;
                            changeStart = true;
                        }
                        else if (key.Key == ConsoleKey.Y)
                        {
                            Constants.startPageY = value;
                            changeStart = true;
                        }
                        else if (key.Key == ConsoleKey.Z)
                        {
                            Constants.startPageZ = value;
                            changeStart = true;
                        }
                    }
                }
                while (!changeStart);
                

            }
        }

        public static void SeeAllOrders()
        {
            using (var myDb = new WebShopContext())
            {
                List<Order> result;
                if (Constants.activeUserId == 1)
                {
                    result = myDb.Orders.ToList();
                }
                else
                {
                    result = myDb.Orders.Where(o => o.CustomerId == Constants.activeUserId).ToList();
                }
                List<string> ordersList = new List<string>();

                if (result.Count == 0)
                {
                    ordersList.Add(Constants.activeUserId == 1 ? "Inga ordrar hittades" : "Du har inga ordrar än");
                }
                else
                {
                    foreach (var order in result)
                    {
                        ordersList.Add($"{order.Id,3}.".PadRight(17) + $"{order.Date}" + $"{order.TotalPrice} kr".PadLeft(30));
                    }
                }

                var ordersWindow = new Window(Constants.activeUserId == 1 ? "Alla ordrar": "Mina ordrar", 30, 10, ordersList);
                ordersWindow.Draw();

                var message = "Vilken order vill du se mer av?: ";
                bool showOrderDetails = false;

                do
                {
                    Console.SetCursorPosition(30, result.Count + 13);
                    Console.WriteLine(message.PadRight(65));
                    Console.SetCursorPosition(30 + message.Length, result.Count + 13);
                    string? checkOrder = Console.ReadLine();
                    bool orderDetails = int.TryParse(checkOrder, out int value);

                    if (!orderDetails)
                    {
                        Console.SetCursorPosition(30 + message.Length, result.Count + 13);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Felaktig inmatning! ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Thread.Sleep(2000);
                    }
                    else if (Constants.activeUserId != 1 && !result.Any(o => o.Id == value)) // Kund kan endast se sina egna ordrar
                    {
                        Console.SetCursorPosition(30 + message.Length, result.Count + 13);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ordern kan inte hittas! ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        OrderService.ShowOrderDetails(value);
                        showOrderDetails = true;
                    }

                }
                while (!showOrderDetails);

            }
        }
        public static void EditOrDeleteProduct(ConsoleKeyInfo key, int productId)
        {
            using (var myDb = new WebShopContext())
            {
                var product = myDb.Products.FirstOrDefault(p => p.Id == productId);
                List<string> editDeleteProduct = new List<string>();
                if (product == null)
                {
                    editDeleteProduct.Add("Ingen produkt hittades");
                    Console.ReadKey();
                    return;
                }

                switch (key.KeyChar) /*[A] Ändra produkt   [B] Ta bort produkt*/
                {
                    case 'A':
                    case 'a':
                        Console.Clear();
                        Helper.Banner();                        
                        
                        
                        editDeleteProduct.Add("Nytt namn (Tomt för att behålla nuvarande): ".PadRight(66));
                        var editDeleteProduct1 = new Window("Redigera vara", 30, 14, editDeleteProduct);
                        editDeleteProduct1.Draw();
                        Console.SetCursorPosition(76, 15);
                        string? newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            product.Name = newName;
                        }
                        Console.SetCursorPosition(32, 15);
                        Console.Write("Nytt pris (Tomt för att behålla nuvarande): ".PadRight(65));
                        Console.SetCursorPosition(76, 15);
                        string? newPriceInput = Console.ReadLine();
                        if (decimal.TryParse(newPriceInput, out decimal newPrice))
                        {
                            product.UnitsPrice = newPrice;
                        }
                        Console.SetCursorPosition(32, 15);
                        Console.Write("Ny lagermängd (Tomt för att behålla nuvarande): ".PadRight(65));
                        Console.SetCursorPosition(80, 15);
                        string? newStockInput = Console.ReadLine();
                        if (int.TryParse(newStockInput, out int newStock))
                        {
                            product.QuantityInStock = newStock;
                        }
                        myDb.Update(product);
                        myDb.SaveChanges();
                        Console.SetCursorPosition(32, 15);
                        Console.WriteLine("Produkten har uppdaterats!".PadRight(65));


                        break;
                    case 'B':
                    case 'b':
                        Console.Clear();
                        Helper.Banner();
                        Console.SetCursorPosition(32, 15);
                        Console.WriteLine($"Är du säker på att du vill ta bort {product.Name}? (J/N) ");
                        ConsoleKeyInfo confirmKey = Console.ReadKey(true);

                        if (char.ToLower(confirmKey.KeyChar) == 'j')
                        {
                            myDb.Products.Remove(product);
                            myDb.SaveChanges();

                            Console.Clear();
                            Helper.Banner();
                            Console.SetCursorPosition(32, 15);
                            Console.WriteLine("Produkten har tagits bort!".PadRight(65));
                        }
                        else
                        {
                            Console.SetCursorPosition(32, 15);
                            Console.WriteLine("Åtgärden avbröts.".PadRight(66));
                        }
                        break;
                }
                Console.ReadKey();
            }
        }


        public static void CreateProduct()
        {
            using (var myDb = new WebShopContext())
            {                
                List<string> createProd = new List<string> {
                "Namn:",
                "Leverantör (Ange ID eller skapa ny):",                
                "Pris:".PadRight(66),
                "Antal i lager:",
                "Beskrivning av produkt:",
                "Kategori-ID (separera med kommatecken om flera): "

                };
                var windowTop = new Window("Skapa produkt", 30, 10, createProd);
                windowTop.Draw();
                Console.SetCursorPosition(38, 11);
                var name = Console.ReadLine();
                name = char.ToUpper(name[0]) + name.Substring(1).ToLower();

                List<string> sup = new List<string>();
                var suppliers = myDb.Suppliers.ToList();
                if (suppliers.Any())
                {
                    foreach (var supplier1 in myDb.Suppliers)
                    {
                        sup.Add($"[{supplier1.Id}] {supplier1.CompanyName}".PadRight(66));
                    }
                }
                if (!suppliers.Any())
                {
                    sup.Add("Inga leverantörer hittades!");
                }
                var sups = new Window("Leverantörer", 30, 18, sup);
                sups.Draw();
                
                
                bool supplierFound = false;
                int supplierId = 0;
                do
                {
                    Console.SetCursorPosition(32, 12);
                    Console.WriteLine("Leverantör (Ange ID eller skapa ny):".PadRight(66));
                    Console.SetCursorPosition(69, 12);
                    var supplier = Console.ReadLine();

                    if (int.TryParse(supplier, out int supplierInput))
                    {
                        var existingSupplier = myDb.Suppliers.FirstOrDefault(s => s.Id == supplierInput);
                        if (existingSupplier != null)
                        {
                            supplierId = supplierInput;
                            Console.SetCursorPosition(69, 12);
                            Console.WriteLine($"{existingSupplier.CompanyName}");
                            supplierFound = true;
                        }
                        else
                        {
                            Console.SetCursorPosition(69, 12);
                            Console.WriteLine("ID finns inte.");
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(69, 12);
                        Console.WriteLine("Ogiltig inmatning.");
                        Thread.Sleep(1000);
                    }
                }
                while (!supplierFound);


                decimal unitPrice;
                string input;
                do
                {
                    Console.SetCursorPosition(32, 13);
                    Console.Write("Pris: ".PadRight(66));
                    Console.SetCursorPosition(38, 13);
                    input = Console.ReadLine();
                }
                while (!decimal.TryParse(input, out unitPrice));

                
                int quantityInStock;
                string input1;
                do
                {
                    Console.SetCursorPosition(32, 14);
                    Console.Write("Antal i lager:".PadRight(66));
                    Console.SetCursorPosition(47, 14);
                    input1 = Console.ReadLine();
                }
                while (!int.TryParse(input1, out quantityInStock));

                Console.SetCursorPosition(56, 15);
                var description = Console.ReadLine();

                List<string> cat = new List<string>();
                var categories = myDb.Categories.ToList();
                if (categories.Any())
                {
                    foreach (var category in myDb.Categories)
                    {
                        cat.Add($"[{category.Id}] {category.Name}".PadRight(66));
                    }
                }
                if (!categories.Any())
                {
                    cat.Add("Inga kategorier hittades!");
                }
                var cats = new Window("Kategorier", 30, 18, cat);
                cats.Draw();


                Console.SetCursorPosition(81, 16);
                var categoryInput = Console.ReadLine();
                var categoryIds = categoryInput.Split(',').Select(c => c.Trim()).ToList();

                var newProduct = new Product
                {
                    Name = name,
                    SupplierId = supplierId, // Förutsätter att leverantören finns i databasen
                    UnitsPrice = unitPrice,
                    QuantityInStock = quantityInStock,
                    Description = description
                };
                foreach (var categoryIdStr in categoryIds)
                {
                    if (int.TryParse(categoryIdStr, out int categoryId))
                    {
                        var category = myDb.Categories.FirstOrDefault(c => c.Id == categoryId);
                        if (category != null)
                        {
                            newProduct.Categories.Add(category);
                        }
                        else
                        {
                            Console.WriteLine($"Kategori med ID '{categoryId}' finns inte i databasen.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ogiltigt kategori-ID: '{categoryIdStr}'");
                    }
                }
                myDb.Products.Add(newProduct);
                myDb.SaveChanges();                
                Console.WriteLine("Produkt är skapad och sparad!");
                Console.ReadKey();
            }
        }
    }
}
