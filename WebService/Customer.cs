using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
    public class Customer
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public double Saldo { get; set; }
        public List<CustomerProduct> Orders { get; set; }

        public Customer() { }

        public Customer(string username, string password, double saldo)
        {
            Username = username;
            Password = password;
            Saldo = saldo;
            Orders = new List<CustomerProduct>();
        }
        public Customer(string username, string password, double saldo, List<CustomerProduct>orders)
        {
            Username = username;
            Password = password;
            Saldo = saldo;
            Orders = orders;
        }
    }
}
