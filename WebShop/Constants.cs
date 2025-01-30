using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebShop
{
    public static class Constants
    {

        public static string activeUser { get; set; } = string.Empty;
        public static int activeUserId { get; set; }
        public static int startPageX { get; set; } = 3;
        public static int startPageY { get; set; } = 6;
        public static int startPageZ { get; set; } = 12;
        public static decimal shippingCost { get; set; }
        public static string adress { get; set; }
        public static string postNumber { get; set; }
        public static string city { get; set; }
        public static string country { get; set; }

    }
}



