using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace WebShop
{
    internal class Program
    {

        static void Main(string[] args)
        {

            bool running = true;
            while (running)
            {

                Helper.Banner();
                Helper.StartPage();
                if (Constants.activeUserId == 1)
                {
                    AdminPage.AdminMenu();                    
                }
                else
                {
                    Helper.Menu();
                    Cart.ShoppingCartSmall();
                }

                
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case 'x':
                    case 'X':
                        if (Constants.activeUserId == 1)
                        {
                            AdminPage.ChangeStartPage(key);
                        }
                        else
                        {
                            Products.ShowProduct(Constants.startPageX);
                        }
                        break;

                    case 'y':
                    case 'Y':
                        if (Constants.activeUserId == 1)
                        {
                            AdminPage.ChangeStartPage(key);
                        }
                        else
                        {
                            Products.ShowProduct(Constants.startPageY);
                        }
                        break;
                    case 'z':
                    case 'Z':
                        
                        if (Constants.activeUserId == 1)
                        {
                            AdminPage.ChangeStartPage(key);                                
                        }
                        else
                        {
                            Products.ShowProduct(Constants.startPageZ);
                        }
                        break;
                    case '1':
                    case '2':
                    case '3':
                        Products.ShowProductsInCategory(int.Parse(key.KeyChar.ToString()));
                        break;
                    case 'S':
                    case 's':
                        Products.SearchProduct();
                        break;
                    case 'L':
                    case 'l':
                        if (Constants.activeUser == "")
                        {
                            User.LogIn();
                        }
                        else
                        {
                            User.LogOut();
                        }
                        break;
                    case 'C':
                    case 'c':
                        if (Constants.activeUser == "")
                        {
                            User.CreateAccount();
                        }
                        else if (Constants.activeUserId == 1)
                        {
                            AdminPage.CreateProduct();
                        }
                        break;
                    case 'v':
                    case 'V':
                        {
                            if (Constants.activeUserId != 0)
                            {
                                Cart.ShoppingCartBig();
                            }
                        }
                        break;
                    case 'Q':
                    case 'q':
                        running = false;
                        break;
                    case 'o':
                    case 'O':
                        AdminPage.SeeAllOrders();
                        break;

                    case 'm':
                    case 'M':
                        Helper.MyPages();
                        break;

                    //case 'a':
                    //    uploadToDb.CreateAdmin();
                    //    break;
                    //case 'b':
                    //    uploadToDb.CreateProducts();
                    //    break;

                }
                Console.Clear();
            }

        }
    }
}
