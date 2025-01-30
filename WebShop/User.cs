using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class User
    {
        
        public static void LogIn()
        {            
            using (var db = new Models.WebShopContext())
            {
                var customer = db.Customers.ToList();
                Console.Write("Ange användarnamn eller E-Post: ");
                var username = Console.ReadLine().ToLower();
                
                bool userFound = false;

                foreach (var item in customer)
                {
                    
                    if (string.Equals(username, item.Email, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(username, item.UserName, StringComparison.OrdinalIgnoreCase))
                    {
                        userFound = true;
                        bool passwordCorrect = false;

                        while (!passwordCorrect)
                        {
                            Console.Write("Ange lösenord: ");
                            Console.ForegroundColor = ConsoleColor.Black;
                            var password = Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            if (password == item.Password)
                            {
                                passwordCorrect = true;
                                if (username == "admin" || username == "admin@snusbus.se")
                                {
                                    Constants.activeUser = item.UserName;
                                    Constants.activeUserId = item.Id;
                                    Console.WriteLine($"Välkommen {item.UserName}");
                                }
                                else
                                {
                                    Constants.activeUser = $"{item.FirstName} {item.LastName}";
                                    Constants.activeUserId = item.Id;
                                    Console.WriteLine($"Välkommen {item.FirstName} {item.LastName}");
                                }
                                Console.ReadKey(true);
                            }
                            else
                            {
                                Console.WriteLine("Felaktigt lösenord! Försök igen");
                            }
                        }
                    }
                    
                }
                if (!userFound)
                {
                    Console.WriteLine("Vi kan inte hitta en användare med det användarnamnet eller E-Post");                    
                    Console.ReadKey(true);
                    Console.Clear();
                }
                
            }
        }
        public static void LogOut()
        {
            Constants.activeUser = string.Empty;
            Constants.activeUserId = 0;
        }
        public static void CreateAccount()
        {
            using (var myDb = new WebShopContext())
            {
                
                int tempNr = 30;
                List<string> topText = new List<string> {                
                "Förnamn:",
                "Efternamn:",
                "E-postadress:                                                     ",
                "Adress:",
                "Postnummer:", 
                "Stad:",
                "Land:", 
                "Mobilnummer:", 
                "Personnummer:",
                "Lösenord:",
                "Upprepa lösenord: " 
                };                
                var windowTop = new Window("Skapa konto", tempNr, 10, topText);
                
                windowTop.Draw();
                Console.SetCursorPosition((tempNr+11), 11);                
                var firstname = Console.ReadLine();
                firstname = char.ToUpper(firstname[0]) + firstname.Substring(1).ToLower();
                Console.SetCursorPosition((tempNr + 13), 12);
                var lastname = Console.ReadLine();
                lastname = char.ToUpper(lastname[0]) + lastname.Substring(1).ToLower();
                string email;
                do
                {
                    Console.SetCursorPosition((tempNr + 16), 13);
                    Console.WriteLine("                                                     ");
                    Console.SetCursorPosition((tempNr + 16), 13);
                    email = Console.ReadLine().ToLower();
                    if (myDb.Customers.Any(c => c.Email == email))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition((tempNr + 16), 13);
                        Console.Write(email + " finns redan i databasen");
                        Console.BackgroundColor= ConsoleColor.Black;
                        Thread.Sleep(2000);
                    }
                }
                while (myDb.Customers.Any(c => c.Email == email)); 
                Console.SetCursorPosition((tempNr + 10), 14);
                var address = Console.ReadLine();
                Console.SetCursorPosition((tempNr + 14), 15);
                var postalCode = Console.ReadLine();
                Console.SetCursorPosition((tempNr + 8), 16);
                var city = Console.ReadLine();
                Console.SetCursorPosition((tempNr + 8), 17);
                var country = Console.ReadLine();
                Console.SetCursorPosition((tempNr + 15), 18);
                var phone = Console.ReadLine();
                Console.SetCursorPosition((tempNr + 16), 19);
                string socialSecurityNumber;
                do
                {
                    Console.SetCursorPosition((tempNr + 16), 19);
                    Console.WriteLine("                                                     ");
                    Console.SetCursorPosition((tempNr + 16), 19);
                    socialSecurityNumber = Console.ReadLine();
                if (myDb.Customers.Any(c => c.SocialSecurityNumber == socialSecurityNumber))
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition((tempNr + 16), 19);
                    Console.Write("Personnumret finns redan i databasen");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Thread.Sleep(2000);
                }
            }
                while (myDb.Customers.Any(c => c.SocialSecurityNumber == socialSecurityNumber));

                string password;
                string password1;
                do
                {
                    Console.SetCursorPosition((tempNr + 12), 20);
                    Console.SetCursorPosition((tempNr + 20), 21);
                    Console.WriteLine("                                                 ");
                    Console.SetCursorPosition((tempNr + 12), 20);
                    Console.ForegroundColor = ConsoleColor.Black;
                    password = Console.ReadLine();
                    Console.SetCursorPosition((tempNr + 20), 21);
                    password1 = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    if (password != password1)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition((tempNr + 20), 21);
                        Console.Write("Lösenorden matchade inte!");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Thread.Sleep(2000);
                    }
                }
                while (password != password1);
                
                myDb.AddRange
                    (
                        new Customer
                        {
                            FirstName = firstname,
                            LastName = lastname,
                            Email = email,
                            Address = address,
                            PostalCode = postalCode,
                            City = city,
                            Country = country,
                            Phone = phone,
                            SocialSecurityNumber = socialSecurityNumber,
                            Password = password

                        }
                    );
                myDb.SaveChanges();
                Console.ReadKey();
            }
        }        
        public static void UpdateCustomerInfo()
        {
            using (var myDb = new WebShopContext())
            {
                var user = myDb.Customers.FirstOrDefault(c => c.Id == Constants.activeUserId);
                List<string> updateCustomer = new List<string>();
                if (user == null)
                {
                    updateCustomer.Add("Ingen användare hittades");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    bool loop = true;
                    while (loop)
                    {
                        updateCustomer.Clear();
                        updateCustomer.AddRange(new[]
                        {
                        $"1. Förnamn: {user.FirstName}",
                        $"2. Efternamn: {user.LastName}",
                        $"3. E-postadress: {user.Email}".PadRight(66),
                        $"4. Adress: {user.Address}",
                        $"5. Postnummer: {user.PostalCode}",
                        $"6. Stad: {user.City}",
                        $"7. Land: {user.Country}",
                        $"8. Mobilnummer: {user.Phone}",
                        $"9. Lösenord: ",
                        "",
                        "",
                        "Vill du uppdatera några uppgifter? Ange ID: "
                    });
                        var userWindow = new Window("Skapa konto", 30, 10, updateCustomer);

                        userWindow.Draw();
                        Console.SetCursorPosition(76, 22);
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        Console.SetCursorPosition(32, 22);
                        switch (key.KeyChar)
                        {
                            case '1':
                                Console.Write("Nytt förnamn: ".PadRight(66));
                                Console.SetCursorPosition(46, 22);
                                user.FirstName = Console.ReadLine();
                                break;

                            case '2':
                                Console.Write("Nytt efternamn: ".PadRight(66));
                                Console.SetCursorPosition(48, 22);
                                user.LastName = Console.ReadLine();
                                break;

                            case '3':
                                Console.Write("Ny e-postadress: ".PadRight(66));
                                Console.SetCursorPosition(49, 22);
                                user.Email = Console.ReadLine();
                                break;

                            case '4':
                                Console.Write("Ny adress: ".PadRight(66));
                                Console.SetCursorPosition(43, 22);
                                user.Address = Console.ReadLine();
                                break;

                            case '5':
                                Console.Write("Nytt postnummer: ".PadRight(66));
                                Console.SetCursorPosition(49, 22);
                                user.PostalCode = Console.ReadLine();
                                break;

                            case '6':
                                Console.Write("Ny stad: ".PadRight(66));
                                Console.SetCursorPosition(41, 22);
                                user.City = Console.ReadLine();
                                break;

                            case '7':
                                Console.Write("Nytt land: ".PadRight(66));
                                Console.SetCursorPosition(43, 22);
                                user.Country = Console.ReadLine();
                                break;

                            case '8':
                                Console.Write("Nytt mobilnummer: ".PadRight(66));
                                Console.SetCursorPosition(50, 22);
                                user.Phone = Console.ReadLine();
                                break;

                            case '9':
                                Console.Write("Nytt lösenord: ".PadRight(66));
                                Console.SetCursorPosition(47, 22);
                                Console.ForegroundColor = ConsoleColor.Black;
                                string? newPassword = Console.ReadLine();
                                Console.SetCursorPosition(32, 22);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("Upprepa lösenord: ".PadRight(66));
                                Console.SetCursorPosition(50, 22);
                                Console.ForegroundColor = ConsoleColor.Black;
                                string? confirmPassword = Console.ReadLine();
                                Console.ForegroundColor = ConsoleColor.White;

                                if (newPassword == confirmPassword)
                                {
                                    user.Password = newPassword;
                                    Console.SetCursorPosition(32, 22);
                                    Console.WriteLine("Lösenord uppdaterat!".PadRight(66));
                                    Thread.Sleep(2000);
                                }
                                else
                                {
                                    Console.SetCursorPosition(32, 22);
                                    Console.WriteLine("Lösenorden matchar inte. Försök igen.".PadRight(66));
                                    Thread.Sleep(2000);
                                }
                                break;
                            case 'x':
                            case 'X':
                                loop = false;
                                break;

                            default:
                                Console.WriteLine("Ogiltigt val, försök igen.".PadRight(66));
                                Thread.Sleep(2000);
                                break;
                        }
                        myDb.SaveChanges();
                    }
                }

            }
        }
    }
}
