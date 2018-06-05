using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Shopping.xaml
    /// </summary>
    public partial class Shopping : Window
    {
        WebService.Customer currentuser = new WebService.Customer();
        WebService.ProductServiceClient WebClient = new WebService.ProductServiceClient("BasicHttpBinding_IProductService");
        public Shopping(WebService.Customer user)
        {
            currentuser = user;
            InitializeComponent();
            shoppingresult.Foreground = Brushes.Red;
            saldo.Text = user.Saldo.ToString();
            foreach(WebService.Product p in WebClient.GetProducts())
            {
                products.Items.Add(p.Name + ": " + p.Price.ToString() + " , " + p.Amount.ToString());
            }
            ResetInventory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            shoppingresult.Text = "";
            if (products.SelectedItem != null) {
                string s = products.SelectedItem.ToString();
                string productname = s.Split(':')[0];
                int pFrom = s.IndexOf(": ") + ": ".Length;
                int pTo = s.LastIndexOf(",");

                double price = Convert.ToDouble(s.Substring(pFrom, pTo - pFrom));
                if (Convert.ToDouble(saldo.Text) - price >= 0)
                {
                    WebClient.BuyProduct(productname, currentuser.Username, currentuser.Password);
                    ResetInventory();
                    saldo.Text = (WebClient.GetUser(currentuser.Username, currentuser.Password).Saldo).ToString();
                    ResetProducts();
                }
                else
                {
                    shoppingresult.Text = "U heeft te weinig saldo";
                }
            }
            else
            {
                shoppingresult.Text = "U heeft geen product geselecteerd";
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ResetProducts();
            shoppingresult.Text = "De productlijst is opnieuw geladen";
        }
        private void ResetProducts()
        {
            products.Items.Clear();
            foreach (WebService.Product p in WebClient.GetProducts())
            {
                products.Items.Add(p.Name + ": " + p.Price.ToString() + " , " + p.Amount.ToString());
            }
        }
        private void ResetInventory()
        {
            inventory.Items.Clear();
            foreach (WebService.CustomerProduct cp in WebClient.GetOrders(currentuser.Username, currentuser.Password))
            {
                inventory.Items.Add(cp.Product.Name + ", " + cp.Amount);
            }
        }
    }
}
