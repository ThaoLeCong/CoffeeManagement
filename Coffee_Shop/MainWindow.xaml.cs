using Coffee_Shop.DAO;
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

namespace Coffee_Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Coffee_ShopEntities data = new Coffee_ShopEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(txtUserName.Text!=""&&txtPassword.Password!="")
            {
                String UserName = txtUserName.Text;
                String Password = txtPassword.Password;
                Login login = new Login();
                if (login.LoginFuntion(UserName, Password))
                {
                    TableManager windowTableManager = new TableManager(login.accountInfor(UserName, Password));
                    this.Hide();
                    windowTableManager.ShowDialog();
                    this.Show();
                }
                else
                    MessageBox.Show("đăng nhập thất bại");
                return;
            }
            else if(txtUserName.Text=="")
            {
                MessageBox.Show("Bạn chưa nhập tên đăng nhập");
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập Password");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         if( MessageBox.Show("bạn có chắc chắn muốn thoát","Thông báo",MessageBoxButton.OKCancel) !=MessageBoxResult.OK)
         {
             e.Cancel = true;
         }
        }
    }
}
