using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class OrderService
    {
        public void CreateOrder(List<ShoppingCart> shoppingCartItems)
        {
            using (var myDb = new WebShopContext())
            {
                var customerId = shoppingCartItems.First().CustomerId;

                List<Product> updatedProduct = new List<Product>();

                foreach (var item in shoppingCartItems.ToList())
                {
                    var product = myDb.Products.FirstOrDefault(p => p.Id == item.ProductId);

                    if (product == null)
                    {
                        Console.WriteLine($"Produkten med ID {item.ProductId} kunde inte hittas.");
                        continue;
                    }

                    if (product.QuantityInStock < item.Quantity)
                    {

                        bool validChoise = false;
                        do
                        {
                            List<string> invalidMenu = new List<string> {
                                $"Det finns inte tillräckligt med {product.Name} i lager.",
                                $"Endast {product.QuantityInStock} kvar.",
                                "",
                            "".PadRight(66),
                            "", ""
                            };
                            var invalidQuantity = new Window("", 30, 10, invalidMenu);
                            invalidQuantity.Draw();
                            Console.SetCursorPosition(32, 16);
                            Console.WriteLine("".PadRight(66));
                           
                            Console.SetCursorPosition(32, 16);
                            Console.Write($"Hur många {product.Name} vill du beställa? (Max {product.QuantityInStock}): ");
                            string? inputQuantity = Console.ReadLine();
                            if (int.TryParse(inputQuantity, out int newQuantity) && newQuantity <= product.QuantityInStock)
                            {
                                item.Quantity = newQuantity;
                                Console.SetCursorPosition(32, 16);
                                Console.WriteLine($"Antalet för {product.Name} har uppdaterats till {newQuantity}.".PadRight(66));
                                Console.ReadKey(true);
                                validChoise = true;
                            }
                            else
                            {
                                Console.SetCursorPosition(32, 16);
                                Console.WriteLine("Ogiltigt antal eller för stort antal!".PadRight(66));
                                Console.ReadKey(true);
                            }
                            
                        }
                        while (!validChoise);

                    }
                    updatedProduct.Add(product);


                }
                var order = new Order    // Skapar ordern
                {
                    CustomerId = customerId,
                    Date = DateTime.Now.Date,
                    ShipAddress = Constants.adress,
                    ShipPostalcode = Constants.postNumber,
                    ShipCity = Constants.city,
                    ShipCountry = Constants.country,
                    ShippingCost = Constants.shippingCost,
                    TotalPrice = shoppingCartItems.Sum(item => item.TotalPrice) + Constants.shippingCost
                };
                myDb.Orders.Add(order);
                myDb.SaveChanges();

                foreach (var item in shoppingCartItems)
                {
                    var product = updatedProduct.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.TotalPrice
                        };
                        product.QuantityInStock -= item.Quantity;
                        myDb.OrderDetails.Add(orderDetail);
                        myDb.Products.Update(product);
                    }
                }

                myDb.shoppingCarts.RemoveRange(shoppingCartItems);
                myDb.SaveChanges();
            }
        }
        public static void ShowOrderDetails(int value)
        {
            using (var myDb = new WebShopContext())
            {
                var order = myDb.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Include(o => o.Customer)

                    .FirstOrDefault(o => o.Id == value);
                List<string> orderDetailsList = new List<string>();
                if (order != null)
                {
                    orderDetailsList.Add($"Kundnummer: {order.CustomerId}".PadRight(66));
                    orderDetailsList.Add($"Namn: {order.Customer.FirstName} {order.Customer.LastName}");
                    orderDetailsList.Add($"Leveransadress: {order.ShipAddress}, {order.ShipPostalcode} {order.ShipCity}");
                    orderDetailsList.Add($"Land: {order.ShipCountry}");
                    orderDetailsList.Add("");
                    foreach (var product in order.OrderDetails)
                    {
                        orderDetailsList.Add($"{product.Quantity} ".PadLeft(4) + $" {product.Product.Name}".PadRight(30) + $"{product.UnitPrice}" + $"{product.TotalPrice} kr".PadLeft(27));

                    }
                    if (order.ShippingCost == 0)
                    {
                        orderDetailsList.Add($"1 ".PadLeft(4) + $" Frakt till ombud".PadRight(31) + $"{order.ShippingCost}" + $"{order.ShippingCost} kr".PadLeft(27));


                    }
                    else
                    {
                        orderDetailsList.Add($"1 ".PadLeft(4) + $" Hemleverans".PadRight(30) + $"{order.ShippingCost}" + $"{order.ShippingCost} kr".PadLeft(27));

                    }
                    orderDetailsList.Add("------------------------------------------------------------------");
                    orderDetailsList.Add("Totalpris inklusive frakt" + $"{order.TotalPrice} kr".PadLeft(41));
                    decimal moms = Math.Round((order.TotalPrice - order.ShippingCost) * 0.2m, 2);
                    orderDetailsList.Add("Moms 25%" + $"{moms} kr".PadLeft(58));
                    var orderDetailsList1 = new Window($"Order {value}", 30, 10, orderDetailsList);
                    orderDetailsList1.Draw();
                    Console.ReadKey();
                }
            }
        }
    }


}
