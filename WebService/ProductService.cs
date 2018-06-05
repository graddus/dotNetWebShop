using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

namespace WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductService" in both code and config file together.
    public class ProductService : IProductService
    {
        CustomerDAO custdao = new CustomerDAO();
        ProductDAO proddao = new ProductDAO();
        
        public ProductService()
        {
        }

        public Product GetProduct(String name)
        {
            Product p = null;
            p = proddao.GetProduct(name);
            return p;
        }
        public bool RegisterUser(string username, string passw)
        {
            bool result = true;
            foreach(Customer c in custdao.GetAllCustomers()){
                if (c.Username.Equals(username))
                {
                    result = false;
                }
            }
            if (result == true)
            {
                custdao.AddCustomer(username, passw);
            }
            return result;
        }
        public Customer GetUser(String username, String password)
        {

            Customer c = null;
            c = custdao.GetCustomer(username, password);
            if (c != null)
            {
                c.Orders = proddao.GetProductsByCustomer(username);
            }
            return c;
        }
        public List<Product> GetProducts()
        {
            return proddao.GetAllProducts();
        }
        public bool BuyProduct(string product, string username, string password)
        {
            foreach (Product pro in proddao.GetAllProducts())
            {
                if (pro.Name.Equals(product) && pro.Amount > 0)
                {
                    proddao.BuyProduct(product, username, password);
                    return true;
                }
            }return false;
        }
        public List<CustomerProduct> GetOrders(string username, string password)
        {
            List<CustomerProduct> result = new List<CustomerProduct>();
            foreach(CustomerProduct cp in GetUser(username, password).Orders)
            {
                result.Add(cp);
            }
            return result;
        }
    }
    }
