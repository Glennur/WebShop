using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebShop;
using WebShop.Models;

namespace WebShop
{
    internal class Products
    {
        public static void ShowProductsInCategory(int key)
        {
            using (var myDb = new WebShopContext())
            {
                var cat = myDb.Categories.FirstOrDefault(c => c.Id == key).Name;
                var result = myDb.Products
                    .Where(p => p.Categories.Any(c => c.Id == key)).ToList();
                List<string> products = new List<string>();
                foreach (var product in result)
                {
                    products.Add($"{product.Id, 3}. {product.Name.PadRight(53)}{product.UnitsPrice:F2} kr");

                }
                var products1 = new Window(cat, 30, 10, products);
                Console.Clear();
                Helper.Banner();
                Helper.Menu();                
                products1.Draw();
                SubMenu(products);
                
            }
        }

        public static void SearchProduct()
        {
            Console.WriteLine("Vad vill du söka efter?: ");
            string? searchProduct = Console.ReadLine();
            string searchLower = searchProduct.ToLower();
            using (var myDb = new WebShopContext())
            {
                var result = myDb.Products
                    .Where(p => p.Name.ToLower().Contains(searchLower)).ToList();
                List<string> products = new List<string>();

                if (searchProduct != null)
                {                    
                    if (result.Count == 0)
                    {
                        products.Add("Inga produkter hittades".PadRight(66));
                        

                    }
                    else
                    {
                        foreach (var product in result)
                        {
                            products.Add($"{product.Id,3}. {product.Name.PadRight(53)}{product.UnitsPrice:F2} kr");
                        }


                    }
                    Console.Clear();
                    Helper.Banner();
                    Helper.Menu();
                    Cart.ShoppingCartSmall();
                    var products1 = new Window(searchProduct, 30, 10, products);
                    products1.Draw();
                    if (result.Count > 0)
                    {
                        SubMenu(products);
                    }
                    else
                    {
                        Console.ReadKey();
                    }

                }
            }
        }
        public static void SubMenu(List<string> products)
        {
            using (var myDb = new WebShopContext())
            {
                int result;
                bool correctProduct = false;

                do
                {
                    List<string> temp = new List<string>() {
                        "Ange ID för produkt: ".PadRight(32)
                };
                    var menu1 = new Window("Meny", 30, 12 + (products.Count), temp);
                    menu1.Draw();

                    List<string> temp1 = new List<string>() {
                        "[ESC]Tillbaka till startsidan"
                };
                    var menu2 = new Window("Meny", 67, 12 + (products.Count), temp1);
                    menu2.Draw();

                    Console.SetCursorPosition(53, 13 + (products.Count));

                    string input = Console.ReadLine();

                    bool showProductId = int.TryParse(input, out result);

                    if (showProductId)
                    {
                        var product = myDb.Products.FirstOrDefault(p => p.Id == result);
                        if (product != null)
                        {
                            ShowProduct(result);
                            correctProduct = true;
                        }                        
                    }
                }
                while (!correctProduct);
            }

        }




        public static void ShowProduct(int result)
        {
            using (var myDb = new WebShopContext())
            {                
                var selectedProduct = myDb.Products.FirstOrDefault(p => p.Id == result);
                List<string> prod = new List<string>();

                int maxWidth = 67; // Maximalt antal tecken per rad
                var editedDescription = EditDescription(selectedProduct.Description, maxWidth);
                prod.Add($"{selectedProduct.UnitsPrice:F2} kr".PadLeft(66));
                prod.Add("");                
                prod.AddRange(editedDescription);
                prod.Add("");
                prod.Add($"Antal i lager: {selectedProduct.QuantityInStock}");
                if (Constants.activeUserId == 1)
                {
                    prod.Add("[A] Ändra produkt   [B] Ta bort produkt".PadLeft(66));
                }
                else
                {
                    prod.Add("[K]Köp".PadLeft(66));
                }


                var prod1 = new Window(selectedProduct.Name, 30, 10, prod);
                prod1.Draw();
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (Constants.activeUserId != 1) // Inte admin
                {
                    switch (key.KeyChar)
                    {
                        case 'k':
                        case 'K':
                            do
                            {
                                Console.SetCursorPosition(32, 10 + (prod.Count));
                                Console.Write("Hur många vill du köpa?: ".PadRight(67));

                                Console.SetCursorPosition(57, 10 + (prod.Count));
                                string? input = Console.ReadLine();
                                bool intInput = int.TryParse(input, out result) && result > 0;

                                if (!intInput)
                                {
                                    Console.SetCursorPosition(57, 10 + (prod.Count));
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.Write("Felaktig inmatning!");
                                    Thread.Sleep(2000);
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    continue; 
                                }

                                Cart.AddToCart(result, selectedProduct.Id, myDb, prod.Count);

                            } while (result == 0);

                            break;
                    }
                }
                else // Admin
                {
                    AdminPage.EditOrDeleteProduct(key, result);
                }

            }
        }
        public static List<string> EditDescription(string text, int maxWidth)
        {
            List<string> lines = new List<string>();
            while (!string.IsNullOrEmpty(text))
            {
                if (text.Length <= maxWidth)
                {
                    lines.Add(text); // Lägg till hela texten om den får plats
                    break;
                }

                // Hitta sista mellanslag innan maxWidth
                int breakIndex = text.LastIndexOf(' ', maxWidth);
                if (breakIndex == -1) breakIndex = maxWidth; // Om inget mellanslag finns, dela vid maxWidth

                lines.Add(text.Substring(0, breakIndex).Trim());
                text = text.Substring(breakIndex).Trim();
            }
            return lines;
        }

    }
}



