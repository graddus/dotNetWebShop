using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
    public class Product
    {
        public string Name
        {
            get;
            set;
        }
        public double Price
        {
            get;
            set;
        }
        public int Amount
        {
            get;
            set;
        }
        public Product(string name,double price, int amount)
        {
            Name = name;
            Price = price;
            Amount = amount;
        }
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
        public Product()
        {
        }
    }
}