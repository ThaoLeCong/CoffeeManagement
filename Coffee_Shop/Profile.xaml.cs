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
using System.Windows.Shapes;

namespace Coffee_Shop
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        TblAccount accountProfile = new TblAccount();
        ProfileModel profile = new ProfileModel();
        public Profile()
        {
            InitializeComponent();
            txtUserName.Text = accountProfile.UserName;
            txtDisplayName.Text = accountProfile.DisplayName;
        }
        public Profile(TblAccount account)
            : this()
        {
            accountProfile = account;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassWord.Password == "")
            {
                MessageBox.Show("Bạn phải nhập mật khẩu cũ để có thể thay đổi thông tin tài khoản");
            }
            else
            {
                if (profile.IsPasswordTrue(accountProfile.ID, txtPassWord.Password))
                {
                    if(txtNewPassWord.Password==""&&txtConfirmPassWord.Password=="")
                    {
                        profile.EditAccount(accountProfile.ID, txtDisplayName.Text,accountProfile.Pass);
                    }
                    else if(txtNewPassWord.Password!=txtConfirmPassWord.Password)
                    {
                        MessageBox.Show("Mật khẩu nhập lại khác với mật khẩu mới");
                    }
                    else
                    {
                        if(MessageBox.Show("Bạn chắc chắn muốn thay đổi", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Question)==MessageBoxResult.OK)
                        {
                            profile.EditAccount(accountProfile.ID, txtDisplayName.Text, txtNewPassWord.Password);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu cũ không đúng. Vui lòng nhập lại");
                }
            }
        }
    }
}
