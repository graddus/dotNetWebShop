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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebService.ProductServiceClient WebClient = new WebService.ProductServiceClient("BasicHttpBinding_IProductService");
        WebService.Customer currentuser = new WebService.Customer();
        public MainWindow()
        {
            InitializeComponent();
            registerresult.Foreground = Brushes.Red;
            loginresult.Foreground = Brushes.Red;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            registerresult.Text = "";
            string username = registerusername.Text;
            string passw = "";
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            passw=new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            passwordgen.Text = passw;
            if (WebClient.RegisterUser(username, passw) != true)
            {
                registerresult.Text="Deze username is al in gebruik";
            }
            else
            {
                WebClient.RegisterUser(username, passw);
                registerresult.Text = "Uw account is succesvol aangemaakt";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            registerresult.Text = "";
            string username = myname.Text;
            string passw = mypassword.Password;
            WebService.Customer mylogin = WebClient.GetUser(username, passw);
            if (mylogin ==null)
            {
                loginresult.Text = "De ingevulde inloggegevens zijn incorrect";
            }
            else
            {
                currentuser = mylogin;
                Shopping shop = new Shopping(mylogin);
                shop.Show();
                this.Close();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
