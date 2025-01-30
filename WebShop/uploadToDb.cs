//using WebShop.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebShop.Models;

//namespace WebShop
//{
//    internal class uploadToDb
//    {
//        public static void CreateAdmin()
//        {
//            using (var myDb = new WebShopContext())
//            {
//                myDb.AddRange
//                    (
//                        new Customer
//                        {
//                            UserName = "admin",
//                            Email = "admin@snusbus.se",
//                            Password = "admin",
//                        }
//                    );
//                myDb.SaveChanges();
//            }
//        }

//        public static void CreateProducts()
//        {
//            using (var myDb = new WebShopContext())
//            {
//                var category1 = new Category { Name = "Portionssnus" };
//                var category2 = new Category { Name = "Lössnus" };
//                var category3 = new Category { Name = "White portion" };

//                var supplier1 = new Supplier { CompanyName = "Swedish Match" };

//                myDb.AddRange(
//                    new Product
//                    {
//                        Name = "General Original Portion",
//                        Categories = new List<Category> { category1 },
//                        Supplier = supplier1,
//                        UnitsPrice = 49.90m,
//                        QuantityInStock = 0,
//                        Description = "General Original Portion är ett portionssnus med kraftig och kryddig tobakssmak. Smaken har peppriga övertoner med toner av bergamott, hö, läder och lite te."
//                    },
//                    new Product
//                    {
//                        Name = "General White Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 49.90m,
//                        QuantityInStock = 1000,
//                        Description = "General White Portion är en vit portion som har en tobakssmak med viss kryddighet. Smaken har peppriga övertoner och en antydan av citrusfrukten bergamott."
//                    },
//                    new Product
//                    {
//                        Name = "General Lös",
//                        Categories = new List<Category> { category2 },
//                        Supplier = supplier1,
//                        UnitsPrice = 55.90m,
//                        QuantityInStock = 1000,
//                        Description = "General Lös är ett lössnus med en kryddig tobakssmak. Smaken har peppriga övertoner och en antydan av citrus. Det här snuset har funnits sedan 1866."
//                    },
//                    new Product
//                    {
//                        Name = "Ettan Lös",
//                        Categories = new List<Category> { category2 },
//                        Supplier = supplier1,
//                        UnitsPrice = 55.90m,
//                        QuantityInStock = 1000,
//                        Description = "Ettan Lös är ett av Sveriges äldsta snus med en traditionell tobakssmak. Ettan Lös tillverkas av mald tobak, är fylligt och du formar den själv till en prilla. Smaken är lätt rökig med en ton av mörk choklad. Ettan lös har en snabb och rejäl smakrelease."
//                    },
//                    new Product
//                    {
//                        Name = "Ettan Original Portion",
//                        Categories = new List<Category> { category1 },
//                        Supplier = supplier1,
//                        UnitsPrice = 49.90m,
//                        QuantityInStock = 1000,
//                        Description = "Ettan Portion från ett sv Sveriges allra äldsta snusmärken, är ett portionssnus med en traditionell tobakssmak med en robust och lätt rökig karaktär med inslag av bland annat tjära och malt. Varje dosa innehåller 24 prillor."
//                    },
//                    new Product
//                    {
//                        Name = "Grov Original Portion",
//                        Categories = new List<Category> { category1 },
//                        Supplier = supplier1,
//                        UnitsPrice = 44.90m,
//                        QuantityInStock = 1000,
//                        Description = "Grov Portion, under varumärket Grov ,har en smakprofil som innefattar tobak, örter, trä, geranium, mandel, tjära."
//                    },
//                    new Product
//                    {
//                        Name = "Grov Lössnus",
//                        Categories = new List<Category> { category2 },
//                        Supplier = supplier1,
//                        UnitsPrice = 55.90m,
//                        QuantityInStock = 1000,
//                        Description = "Grov Lössnus är ett lössnus med traditionell tobakssmak. Grov Lös har en ren smak av tobak och en avrundad eftersmak med inslag av torkade örter och trä."
//                    },
//                    new Product
//                    {
//                        Name = "Grov White Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 44.90m,
//                        QuantityInStock = 1000,
//                        Description = "Grov White Portionssnus erbjuder en smakprofil bestående av traditionell tobak och inslag av bland annat geranium, örter, trä, tjära och mandel i ett normal-format med 24 portioner per dosa."
//                    },
//                    new Product
//                    {
//                        Name = "Göteborgs Rapé Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 34.90m,
//                        QuantityInStock = 1000,
//                        Description = "Göteborgs Rapé, tillverkad av Swedish Match har smak av tobak med inslag av lavendel, citrus, ceder, enbär. Prillorna är av normalformat med 24 portioner per dosa."
//                    },
//                    new Product
//                    {
//                        Name = "Göteborgs Rapé Original Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 34.90m,
//                        QuantityInStock = 1000,
//                        Description = "Göteborgs Rapé Original Large Portion från Göteborgs Rapé erbjuder en smak av traditionell tobak, lavendel, citrus, ceder, enbär i normalformat med 24 prillor per dosa och normal styrka."
//                    },
//                    new Product
//                    {
//                        Name = "Göteborgs Rapé Lössnus",
//                        Categories = new List<Category> { category2 },
//                        Supplier = supplier1,
//                        UnitsPrice = 55.90m,
//                        QuantityInStock = 1000,
//                        Description = "Göteborgs Rapé Lössnus har en smakprofil som innefattar klassisk tobak med inslag av citrus, lavendel, ceder och enbär. Produkten harnormal nikotinhalt ich vikten per dosa är 42 gram grovkornigt lössnus."
//                    },
//                    new Product
//                    {
//                        Name = "One Blå White Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 31.90m,
//                        QuantityInStock = 1000,
//                        Description = "One Blå White Portion från One innehåller aromer av tobak, lavendel, trä, citrus och enbär, klassificerad som stark, i ett standardformat med 20 prillor per dosa."
//                    },
//                    new Product
//                    {
//                        Name = "One Svart White Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 31.90m,
//                        QuantityInStock = 1000,
//                        Description = "ONE Svart White Portion från One innehåller en blandning av tobak, bergamott, citrus och te. Produkten har 20 portioner per dosa och klassificeras som stark."
//                    },
//                    new Product
//                    {
//                        Name = "One Svart Original Portion",
//                        Categories = new List<Category> { category1 },
//                        Supplier = supplier1,
//                        UnitsPrice = 31.90m,
//                        QuantityInStock = 1000,
//                        Description = "ONE Svart Original Portion från ONE snus har smaker av tobak, bergamott, citrus och te. Produkten har 20 portioner per dosa och klassificeras som stark."
//                    },
//                    new Product
//                    {
//                        Name = "One Röd White Portion",
//                        Categories = new List<Category> { category1, category3 },
//                        Supplier = supplier1,
//                        UnitsPrice = 31.90m,
//                        QuantityInStock = 1000,
//                        Description = "ONE Röd White Portion from ONE snus har smaker av tobak, blommor, röda bär och grapefrukt. Produkten har 20 portioner per dosa och klassificeras som stark."
//                    }

//                    );
//                myDb.SaveChanges();
//            }

//        }
//    }
//}
