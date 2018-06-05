using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Diagnostics;
using System.Data;

namespace WebService
{
    public class CustomerDAO : BaseDAO
    {

        public CustomerDAO() : base()
        {
        }
        public Customer GetCustomer(string username, string pass)
        {
            OracleConnection conn = connection; 
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from customers where username = '" + username + "' AND password='"+pass+"'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                string name = dr["username"].ToString();
                string password = dr["password"].ToString();
                double saldo = Convert.ToDouble(dr["saldo"].ToString());
                Customer customer = new Customer(name, password, saldo);
                Debug.WriteLine(customer.Password);
                return customer;
            }
            conn.Close();
            return null;
        }
        public List<Customer> GetAllCustomers()
        {
            OracleConnection conn = connection;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from customers";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            List<Customer> customerlist = new List<Customer>();
            while (dr.Read())
            {
                string name = dr["username"].ToString();
                string password = dr["password"].ToString();
                double saldo = Convert.ToDouble(dr["saldo"].ToString());
                Customer customer = new Customer(name, password, saldo);
                customerlist.Add(customer);
            }
            conn.Close();
            return customerlist;
        }
        public Boolean AddCustomer(string username, string password)
        {
            OracleConnection conn = connection;
            conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                Debug.WriteLine("Writing Text");
                cmd.CommandText = "Insert into Customers VALUES ('"+username+"','"+password+"',50)";
                Debug.WriteLine("Executing");
                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated == 0) {
                    conn.Dispose();
                    return false;
                }

                conn.Close();
                return true;
            }
    }
    }
