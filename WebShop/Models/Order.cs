using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipPostalcode { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipCountry { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalPrice { get; set; }
        

        public virtual Customer? Customer { get; set; }       
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
