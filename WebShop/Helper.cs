using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
//using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace WebShop
{
    internal class Helper
    {
        public static void Banner()
        {
            Console.WriteLine(Constants.activeUser);
            List<string> topText = new List<string> {
                "  ███████╗███╗   ██╗██╗   ██╗███████╗    ██████╗ ██╗   ██╗███████╗",
                "  ██╔════╝████╗  ██║██║   ██║██╔════╝    ██╔══██╗██║   ██║██╔════╝",
                "  ███████╗██╔██╗ ██║██║   ██║███████╗    ██████╔╝██║   ██║███████╗",
                "  ╚════██╗██║╚██╗██║██║   ██║╚════██╗    ██╔══██╗██║   ██║╚════██╗",
                "  ██████╔╝██║ ╚████║╚██████╔╝██████╔╝    ██████╔╝╚██████╔╝██████╔╝",
                "  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚═════╝     ╚═════╝  ╚═════╝ ╚═════╝", "" };
            Console.ForegroundColor = ConsoleColor.Cyan;
            var windowTop = new Window("", 30, 1, topText);

            windowTop.Draw();
            Console.SetCursorPosition(50, 8);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("SNUS & BUS - Smaken av frihet");
            Console.ResetColor();
        }
        public static void StartPage()
        {
            List<string> startPageX = new List<string>();
            List<string> startPageY = new List<string>();
            List<string> startPageZ = new List<string>();
            using (var db = new Models.WebShopContext())
            {
                var productName = db.Products.ToList();
                foreach (var product in productName)
                {
                    if (product.Id == Constants.startPageX)
                    {
                        startPageX.Add(product.Name.Length > 15 ? product.Name.Substring(0, 15) + "..." : product.Name);
                        startPageX.Add($"{product.UnitsPrice:F2} kr");
                        if (Constants.activeUserId == 1)
                        {
                            startPageX.Add("X för att ändra".PadRight(18));
                        }
                        else
                        {
                            startPageX.Add("X för att se mer".PadRight(18));
                        }
                    }
                    if (product.Id == Constants.startPageY)
                    {
                        startPageY.Add(product.Name.Length > 15 ? product.Name.Substring(0, 15) + "..." : product.Name);
                        startPageY.Add($"{product.UnitsPrice:F2} kr");
                        if (Constants.activeUserId == 1)
                        {
                            startPageY.Add("Y för att ändra".PadRight(18));
                        }
                        else
                        {
                            startPageY.Add("Y för att se mer".PadRight(18));
                        }
                        
                    }
                    if (product.Id == Constants.startPageZ)
                    {
                        startPageZ.Add(product.Name.Length > 15 ? product.Name.Substring(0, 15) + "..." : product.Name);
                        startPageZ.Add($"{product.UnitsPrice:F2} kr");
                        if (Constants.activeUserId == 1)
                        {
                            startPageZ.Add("Z för att ändra".PadRight(18));
                        }
                        else
                        {
                            startPageZ.Add("Z för att se mer".PadRight(18));
                        }
                        
                    }
                }
            }
            var startPage1 = new Window("X", 30, 10, startPageX);
            var startPage2 = new Window("Y", 54, 10, startPageY);
            var startPage3 = new Window("Z", 78, 10, startPageZ);
            startPage1.Draw();
            startPage2.Draw();
            startPage3.Draw();


        }
        public static void Menu()
        {
            List<string> menu = new List<string>();
            using (var db = new Models.WebShopContext())
            {
                var categories = db.Categories.ToList();
                if (Constants.activeUser == "")
                {
                    menu.Add("[L] Logga in");
                    menu.Add("[C] Skapa konto");
                }
                else
                {
                    menu.Add("[L] Logga ut");
                }
                menu.Add("[M] Mina sidor");
                menu.Add("[S] Sök på vara");
                if (Constants.activeUser != "admin")
                {
                    if (categories.Any())
                    {
                        foreach (var category in db.Categories)
                        {
                            menu.Add($"[{category.Id}] {category.Name}");
                        }
                    }
                    if (!categories.Any())
                    {
                        menu.Add("Inga kategorier hittades!");
                    }
                }
                menu.Add("[Q] Avsluta");
            }
            var windowTop = new Window("", 0, 10, menu);
            windowTop.Draw();
        }

        public static void MyPages()
        {
            Console.Clear();
            Banner();
            MyPagesMenu();
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.KeyChar)
            {
                case 'c':
                case 'C':
                    User.UpdateCustomerInfo();

                    break;
                case 'o':
                case 'O':
                    AdminPage.SeeAllOrders();

                    break;
            }      
        }
        public static void MyPagesMenu()
        {
            {
                List<string> myPagesMenu = new List<string>();  
                    
                    
                myPagesMenu.Add("[C] Ändra uppgifter");
                myPagesMenu.Add("[O] Visa ordrar");

                myPagesMenu.Add("[X] Tillbaka till start");

                var windowTop = new Window("", 0, 10, myPagesMenu);
                windowTop.Draw();
            }
        }

        public static void CheckOut()
        {
            Console.Clear();
            Helper.Banner();
            Helper.Menu();
            Cart.ShoppingCartSmall();


            using (var myDb = new Models.WebShopContext())
            {
                var loggedInUser = myDb.Customers.FirstOrDefault(c => c.Id == Constants.activeUserId);
                Constants.adress = loggedInUser.Address;
                Constants.postNumber = loggedInUser.PostalCode;
                Constants.city = loggedInUser.City;
                Constants.country = loggedInUser.Country;
                bool checkOutFinished = false;

                do
                {
                    List<string> checkOut = new List<string>();

                    checkOut.Add("1. Adress:".PadRight(23) + $"{Constants.adress}");
                    checkOut.Add($"2. Postnummer och ort: {Constants.postNumber} {Constants.city}");
                    checkOut.Add($"3. Land:".PadRight(22) + $" {Constants.country}");
                    checkOut.Add("");
                    checkOut.Add("Stämmer uppgifterna? Ange Id för ändra den posten: ".PadRight(66));

                    var checkOut1 = new Window("Leveransadress", 30, 10, checkOut);
                    checkOut1.Draw();
                    Console.SetCursorPosition(83, 15);
                    var updateCustomer = Console.ReadLine();
                    if (updateCustomer == "1" || updateCustomer == "2" || updateCustomer == "3")
                    {
                        if (updateCustomer == "1")
                        {
                            Console.SetCursorPosition(32, 15);
                            Console.Write("Ange ny adress: ".PadRight(65));
                            Console.SetCursorPosition(48, 15);
                            Constants.adress = Console.ReadLine();
                        }
                        if (updateCustomer == "2")
                        {

                            Console.SetCursorPosition(32, 15);
                            Console.Write("Ange nytt Postnummer: ".PadRight(65));
                            Console.SetCursorPosition(54, 15);
                            var postNumber = Console.ReadLine();
                            Console.SetCursorPosition(32, 15);
                            Console.Write("Ange ny stad: ".PadRight(65));
                            Console.SetCursorPosition(46, 15);
                            var city = Console.ReadLine();
                            Constants.postNumber = postNumber;
                            Constants.city = city;
                        }
                    }
                    else if (updateCustomer == "")
                    {
                        checkOutFinished = true;
                    }
                }
                while (!checkOutFinished);

                Shipping();
            }
        }
        public static void Shipping()
        {
            Console.SetCursorPosition(31, 17);
            Console.WriteLine("[H]Hemleverans med EarlyBird - 49 kr".PadRight(39) + "[O]Leverans till ombud - 0 kr");
            Console.SetCursorPosition(31, 19);
            Console.Write("Hur vill du ha varorna levererade?: ");
            bool shipping = false;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case 'h':
                    case 'H':
                        Console.SetCursorPosition(31, 19);
                        Console.WriteLine("Hemleverans valt som fraktmetod. 49 kr kostar detta");
                        Constants.shippingCost = 49;
                        Thread.Sleep(2000);
                        shipping = true;
                        Payment();
                        break;
                    case 'o':
                    case 'O':
                        Console.SetCursorPosition(31, 19);
                        Console.WriteLine("Leverans till närmaste ombud är valt. Detta alternativ är gratis");
                        Constants.shippingCost = 0;
                        Thread.Sleep(2000);
                        Payment();
                        shipping = true;
                        break;
                }
            }
            while (!shipping);
        }
        public static void Payment()
        {
            Console.SetCursorPosition(31, 19);
            Console.WriteLine("Vill du betala med [K] kort eller [F] faktura? ".PadRight(67));
            bool paid = false;
            using (var db = new Models.WebShopContext())
            { 
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);                    
                    switch (key.KeyChar)
                    {
                        case 'k':
                        case 'K':
                            Console.SetCursorPosition(31, 19);
                            ShowPaymentMessage("Kortbetalning vald. Tack för ditt köp!");                            
                            paid = true;

                            break;
                        case 'f':
                        case 'F':
                            Console.SetCursorPosition(31, 19);
                            ShowPaymentMessage("Fakturabetalning vald. Fakturan kommer via mail. Tack för ditt köp!");                            
                            paid = true;
                            break;
                    }

                }
                while (!paid); 
            }
        }
        public static void PaymentDone()
        {
            using (var myDb = new Models.WebShopContext())
            {
                var shoppingCartItems = myDb.shoppingCarts.Where(sc => sc.CustomerId == Constants.activeUserId).ToList();

                if (!shoppingCartItems.Any())
                {
                    Console.WriteLine("Kundkorgen är tom. Ordern kunde inte behandlas");
                    return;
                }

                var orderService = new OrderService();
                orderService.CreateOrder(
                    shoppingCartItems                    
                    );
                Constants.shippingCost = 0;
                Constants.adress = "";
                Constants.postNumber = "";
                Constants.city = "";
                Constants.country = "";
            }
            
        }

        public static void ShowPaymentMessage(string message)
        {
            Console.SetCursorPosition(31, 19);
            Console.WriteLine(message);
            PaymentDone();
            Thread.Sleep(5000);
        }     
    }
}
