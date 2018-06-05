using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
    public class CustomerProduct
    {
        public Product Product { get; set; }
        public int Amount { get; set; }

        public CustomerProduct() { }
        public CustomerProduct(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }
    }
}
