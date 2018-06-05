using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace WebService
{
    public class ProductDAO : BaseDAO
    {
        private CustomerDAO custdao = new CustomerDAO();

        public ProductDAO() : base()
        {
        }
        public Product GetProduct(string prodname)
        {
            OracleConnection conn = connection;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from products where name = '" + prodname + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                string name = dr["name"].ToString();
                double price = Convert.ToDouble(dr["price"].ToString());
                int amount = Int32.Parse((dr["stock"].ToString()));
                Product product = new Product(name, price, amount);
                conn.Close();
                return product;
            }
            conn.Close();
            return null;
        }
        public List<Product> GetAllProducts()
        {
            OracleConnection conn = connection;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from products";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            List<Product> productlist = new List<Product>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string name = dr["name"].ToString();
                    double price = Convert.ToDouble(dr["price"].ToString());
                    int amount = Int32.Parse((dr["stock"].ToString()));
                    Product product = new Product(name, price, amount);
                    if (product.Amount > 0)
                    {
                        productlist.Add(product);
                    }
                }
            }
            conn.Close();
            return productlist;
        }
        public bool BuyProduct(string product, string username, string password)
        {
            OracleConnection conn = connection;
            Product p = GetProduct(product);
            Customer user = custdao.GetCustomer(username, password);
            user.Orders = GetProductsByCustomer(username);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            if (p.Amount > 0 && user.Saldo >= p.Price)
            {

                cmd.Connection = conn;
                cmd.CommandText = "Update Products Set Stock=Stock-1 Where Name='" + product + "'";
                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated == 0)
                {
                    conn.Close();
                    return false;
                }
                conn.Close();
                AddToInventory(p, user);
                return true;
            }
            conn.Close();
            return false;
        }
        public bool AddToInventory(Product p, Customer user)
        {
            OracleConnection conn = connection;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            if (p.Amount > 0)
            {
                cmd.Connection = conn;
                cmd.CommandText = "Update Customers_Products Set Amount=Amount+1 Where product_name='" + p.Name + "' AND customer_name='" + user.Username + "'";
                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated == 0)
                {
                    cmd.CommandText = "Insert into Customers_products Values('" + user.Username + "','" + p.Name + "',1)";
                    rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated == 0)
                    {
                        conn.Close();
                        return false;
                    }
                    MinSaldo(user, p.Price);
                    conn.Close();
                    return true;
                }
                MinSaldo(user, p.Price);
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }
        public List<CustomerProduct> GetProductsByCustomer(string username)
        {
            OracleConnection conn = connection;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from customers_products where customer_name = '" + username + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            List<CustomerProduct> productlist = new List<CustomerProduct>();
            while (dr.Read())
            {
                string productname = dr["product_name"].ToString();
                int amount = Int32.Parse(dr["amount"].ToString());
                Product p = null;

                OracleCommand cmd2 = new OracleCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = "select * from products where name = '" + productname + "'";
                cmd2.CommandType = CommandType.Text;
                OracleDataReader dr2 = cmd2.ExecuteReader();

                if (dr2.Read())
                {
                    string name = dr2["name"].ToString();
                    double price = Convert.ToDouble(dr2["price"].ToString());
                    int stock = Int32.Parse((dr2["stock"].ToString()));
                    p = new Product(name, price, stock);
                }
                CustomerProduct cp = new CustomerProduct(p, amount);
                productlist.Add(cp);
            }
            conn.Close();
            return productlist;
        }

        public bool MinSaldo(Customer customer, double cost)
        {
            OracleConnection conn = connection;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Update customers set saldo=saldo-" + cost + " where username='" + customer.Username + "'";
            int rowsUpdated = cmd.ExecuteNonQuery();
            if (rowsUpdated == 0)
            {
                conn.Dispose();
                return false;
            }
            return true;
        }
    }
}