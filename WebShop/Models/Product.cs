using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SupplierId { get; set; }
        public decimal UnitsPrice { get; set; }
        public int QuantityInStock { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual Supplier? Supplier { get; set; }

    }
}
