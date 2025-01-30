using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class Cart
    {
        public static void AddToCart(int quantity, int productId, WebShopContext myDb, int prodCount)
        {
            if (quantity > 0)
            {
                var existingCartItem = myDb.shoppingCarts
                                    .FirstOrDefault(sc => sc.CustomerId == Constants.activeUserId && sc.ProductId == productId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                    myDb.Update(existingCartItem);
                }
                else
                {
                    var newCartItem = new ShoppingCart
                    {
                        CustomerId = Constants.activeUserId,
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = myDb.Products.FirstOrDefault(p => p.Id == productId)?.UnitsPrice ?? 0
                    };
                    myDb.shoppingCarts.Add(newCartItem);
                }
                try
                {
                    myDb.SaveChanges();
                }
                catch { Console.WriteLine("Du måste vara inloggad för att lägga till i snuskorgen!"); }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(55, 13 + (prodCount));
                Console.Write("Felaktig inmatning!");
                Thread.Sleep(2000);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        public static void ShoppingCartSmall()
        {
            int posSmallInfo = 5;
            int posTotalPrice = 23;
            int posProdName = 29;
            int posUnitTotalPrice = 13;
            int posPrice = 2;
            int posXWindow = 105;
            int posYWindow = 1;
            string cart = "small";

            ShoppingCart(posSmallInfo, posTotalPrice, posProdName, posUnitTotalPrice, posPrice, posXWindow, posYWindow, cart);
        }
        public static void ShoppingCartBig()
        {
            Console.Clear();
            Helper.Banner();
            Helper.Menu();
            int posSmallInfo = 5;
            int posTotalPrice = 29;
            int posProdName = 35;
            int posUnitTotalPrice = 17;
            int posUnitPrice = 2;
            int posXWindow = 30;
            int posYWindow = 10;
            string cart = "big";

            ShoppingCart(posSmallInfo, posTotalPrice, posProdName, posUnitTotalPrice, posUnitPrice, posXWindow, posYWindow, cart);
            
        }
        public static void ShoppingCart(int posSmallInfo, int posTotalPrice, int posProdName, int posUnitTotalPrice, int posUnitPrice, int posXWindow, int posYWindow, string cart)
        {

            using (var myDb = new WebShopContext())
            {
                var carts = new List<string>();
                var cartItems = myDb.shoppingCarts
                                .Where(sc => sc.CustomerId == Constants.activeUserId)
                                .ToList();
                decimal cartTotalPrice = 0;
                if (cartItems.Count == 0)
                {
                    carts.Add($"Snuskorgen är tom".PadRight(30));
                    
                }
                else
                {
                    foreach (var item in cartItems.OrderBy(item => item.ProductId))
                    {
                        var product = myDb.Products.FirstOrDefault(p => p.Id == item.ProductId);
                        if (product != null)
                        {
                            if (cart == "small")
                            {
                                carts.Add($"{item.Quantity}".PadLeft(3) +
                                    $" {product.Name.PadRight(posProdName)}" +
                                    $"{item.UnitPrice:F2}".PadLeft(posUnitPrice) + $"{item.TotalPrice:F2} kr".PadLeft(posUnitTotalPrice));
                                cartTotalPrice += item.TotalPrice;
                            }
                            if (cart == "big")
                            {
                                carts.Add($"{item.ProductId}.".PadLeft(3) + $"{item.Quantity}".PadLeft(3) +
                                    $" - {product.Name.PadRight(posProdName)}" +
                                    $"{item.UnitPrice:F2}".PadLeft(posUnitPrice) + $"{item.TotalPrice:F2} kr".PadLeft(posUnitTotalPrice));
                                cartTotalPrice += item.TotalPrice;
                            }


                        }

                    }
                    carts.Add("");
                    if (cart == "small")
                    {
                        carts.Add($"[V] Ändra snuskorg/Checka ut".PadRight(posSmallInfo) + $"Totalt: {cartTotalPrice:F2} kr".PadLeft(posTotalPrice));
                    }
                    if (cart == "big")
                    {
                        carts.Add($"[C]Gå till kassan       [E]Ändra vara" + $"Totalt: {cartTotalPrice:F2} kr".PadLeft(posTotalPrice));
                        
                    }


                    var products2 = new Window("Snuskorg", posXWindow, posYWindow, carts);

                    products2.Draw();

                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.KeyChar)
                    {
                        case 'e':
                        case 'E':
                            bool outerLoop = false;
                            bool innerLoop = false;


                            do
                            {
                                int result = 0;
                                int result1 = 0;
                                Console.SetCursorPosition(30, posYWindow + (carts.Count) + 2);
                                Console.Write("Vilken vara vill du ändra? Ange id: ".PadRight(69));
                                Console.SetCursorPosition(66, posYWindow + (carts.Count) + 2);
                                string? input = Console.ReadLine();
                                bool searchProduct = int.TryParse(input, out result);
                                var changeProduct = cartItems.FirstOrDefault(p => p.ProductId == result);
                                if (!searchProduct)
                                {
                                    Console.SetCursorPosition(66, posYWindow + (carts.Count) + 2);
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Felaktig inmatning!");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Thread.Sleep(2000);
                                }
                                else if (changeProduct == null)
                                {
                                    Console.SetCursorPosition(66, posYWindow + (carts.Count) + 2);
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Varan hittades inte! Försök igen");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Thread.Sleep(2000);
                                }
                                else
                                {
                                    outerLoop = true;
                                    do
                                    {
                                        Console.SetCursorPosition(30, posYWindow + (carts.Count) + 3);
                                        Console.Write($"Hur många {changeProduct.ProductId} vill du ändra till? ");
                                        string? input1 = Console.ReadLine();
                                        bool changeAmount = int.TryParse(input1, out result1);
                                        if (!changeAmount || result1 < 0)
                                        {
                                            Console.SetCursorPosition(30, posYWindow + (carts.Count) + 3);
                                            Console.WriteLine("Felaktig inmatning!");
                                            Thread.Sleep(1000);
                                        }
                                        else if (result1 == 0)
                                        {
                                            innerLoop = true;
                                            Console.WriteLine("Produkten har tagits bort från kundkorgen! ");
                                            myDb.Remove(changeProduct);
                                            Thread.Sleep(1000);
                                        }
                                        else
                                        {
                                            innerLoop = true;
                                            changeProduct.Quantity = result1;
                                            myDb.Update(changeProduct);
                                        }
                                    }
                                    while (!innerLoop);

                                }

                            }
                            while (!outerLoop);
                            myDb.SaveChanges();
                            break;
                        case 'c':
                        case 'C':
                            Helper.CheckOut();
                            break;



                    }
                }
                var products1 = new Window("Snuskorg", posXWindow, posYWindow, carts);

                products1.Draw();
            }


        }
    }
}
