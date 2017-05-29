using Coffee_Shop.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        Admin admin = new Admin();
        Coffee_ShopEntities data = new Coffee_ShopEntities();
        public AdminWindow()
        {
            InitializeComponent();
            Load_dtgFood2();
            Load_dtgFoodCategory();
            Load_dtgTable();
            Load_dtgAccount();
            Load_cbbFoodCategoryName();
            Load_cbbTableStatus();
            Load_cbbTableType();
            Load_cbbAccountType();
            Load_dtgBill();
        }

        #region WindowLoadFunction
        private void Load_dtgFood()
        {
            dtgFood.ItemsSource = admin.getListFood();
        }

        //Get danh sách món ăn (ID,Name,CategoryName,Price)
        private void Load_dtgFood2()
        {
            dtgFood.ItemsSource = admin.GetListFoodWithCategoryName();
        }

        private void Load_dtgFoodCategory()
        {
            dtgFoodCategory.ItemsSource = admin.getListCategory();
        }

        private void Load_dtgTable()
        {
            dtgTable.ItemsSource = admin.getListTable();
        }

        private void Load_dtgAccount()
        {
            dtgAccount.ItemsSource = admin.getListAccount();
        }

        private void Load_dtgBill()
        {
            dtgBill.ItemsSource = admin.GetListBill();
        }
        #endregion

        #region ComboboxLoadFunction
        private void Load_cbbFoodCategoryName()
        {
            cbbFoodCategoryName.ItemsSource = admin.getListCategory().Select(n => n.Name);
        }

        private void Load_cbbTableStatus()
        {
            List<string> tableStatus = new List<string>()
            {
                "Trống","Đã đặt","Có khách"
            };
            cbbTableStatus.ItemsSource = tableStatus;
        }

        private void Load_cbbTableType()
        {
            List<string> tableType = new List<string>()
            {
                "2 người","4 người","6 người","8 người","10 người","12 người","VIP"
            };
            cbbTableType.ItemsSource = tableType;
        }

        private void Load_cbbAccountType()
        {
            List<string> accountType = new List<string>()
            {
                "admin","staff","guess"
            };
            cbbAccountType.ItemsSource = accountType;
        }
        #endregion

        #region FoodEvent
        private void btnAddFood_Click(object sender, RoutedEventArgs e)
        {
            if (txtFoodID.Text != "" && txtFoodName.Text != "" && cbbFoodCategoryName.Text != "" && txtFoodPrice.Text != "")
            {
                int CategoryID = admin.GetCategoryIDByName(cbbFoodCategoryName.Text);
                admin.AddFood(txtFoodName.Text, CategoryID, float.Parse(txtFoodPrice.Text));
                Load_dtgFood2();
            }
            else if (txtFoodName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới món ăn không thành công");
            }
            else if (cbbFoodCategoryName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập danh mục món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới món ăn không thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập giá món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới món ăn không thành công");
            }
        }

        private void btnDeleteFood_Click(object sender, RoutedEventArgs e)
        {
            if (txtFoodID.Text != "" && txtFoodName.Text != "")
            {
                admin.DeleteFood(int.Parse(txtFoodID.Text), txtFoodName.Text);
                Load_dtgFood2();
            }
            else if (txtFoodName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Xóa món ăn không thành công");
            }
        }

        private void btnEditFood_Click(object sender, RoutedEventArgs e)
        {
            if (txtFoodID.Text != "" && txtFoodName.Text != "" && cbbFoodCategoryName.Text != "" && txtFoodPrice.Text != "")
            {
                int CategoryID = admin.GetCategoryIDByName(cbbFoodCategoryName.Text);
                admin.EditFood(int.Parse(txtFoodID.Text), txtFoodName.Text, CategoryID, float.Parse(txtFoodPrice.Text));
                Load_dtgFood2();
            }
            else if (txtFoodName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa món ăn không thành công");
            }
            else if (cbbFoodCategoryName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập danh mục món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa món ăn không thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập giá món ăn", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa món ăn không thành công");
            }
        }

        private void btnFindFood_Click(object sender, RoutedEventArgs e)
        {
            if (txtFindFood.Text != "")
            {
                List<TblFood> listFood = new List<TblFood>();
                listFood = admin.FindFoodByKey(txtFindFood.Text);
                dtgFood.ItemsSource = listFood;
                dtgFood.ItemsSource = admin.FindFoodByKeyWithCategoryName(txtFindFood.Text);
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập từ khóa tìm kiếm", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnSelectFood_Click(object sender, RoutedEventArgs e)
        {
            Load_dtgFood2();
        }
        #endregion

        #region CategoryEvent
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (txtCategoryName.Text != "")
            {
                admin.AddCategory(txtCategoryName.Text);
                Load_dtgFoodCategory();
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tên danh mục", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới danh mục không thành công");
            }
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (txtCategoryName.Text != "")
            {
                admin.DeleteCategory(int.Parse(txtCategoryID.Text), txtCategoryName.Text);
                Load_dtgFoodCategory();
            }
            else
            {

                MessageBox.Show("Tên danh mục cần xóa trống", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Xóa danh mục không thành công");

            }
        }

        private void btnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (txtCategoryName.Text != "")
            {
                admin.EditCategory(int.Parse(txtCategoryID.Text), txtCategoryName.Text);
                Load_dtgFoodCategory();
            }
            else
            {
                MessageBox.Show("Tên danh mục chỉnh sửa trống", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa danh mục không thành công");
            }
        }

        private void btnFindCategory_Click(object sender, RoutedEventArgs e)
        {
            if (txtFindCategory.Text != "")
            {
                List<TblFoodCategory> listCategory = new List<TblFoodCategory>();
                listCategory = admin.FindCategoryByKey(txtFindCategory.Text);
                dtgFoodCategory.ItemsSource = listCategory;
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập từ khóa tìm kiếm", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Tìm kiếm danh mục không thành công");
            }
        }

        private void btnSelectCategory_Click(object sender, RoutedEventArgs e)
        {
            Load_dtgFoodCategory();
        }
        #endregion

        #region TableEvent
        private void btnAddTable_Click(object sender, RoutedEventArgs e)
        {
            if (txtTableName.Text != "" && cbbTableStatus.Text != "" && txtTableLocation.Text != "" && cbbTableType.Text != "")
            {
                admin.AddTable(txtTableName.Text, cbbTableStatus.Text, cbbTableType.Text, txtTableLocation.Text);
                Load_dtgTable();
            }
            else if (txtTableName.Text == "")
            {
                MessageBox.Show("Tên bàn trống, bạn không thể thêm vào", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới bàn ăn không thành công");
            }
            else if (cbbTableStatus.Text == "")
            {
                MessageBox.Show("trạng thái bàn trống, bạn không thể thêm vào", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới bàn ăn không thành công");
            }
            else if (txtTableLocation.Text == "")
            {
                MessageBox.Show("Vị trí bàn trống, bạn không thể thêm vào", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới bàn ăn không thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn loại bàn, bạn không thể thêm vào", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới bàn ăn không thành công");
            }
        }

        private void btnDeleteTable_Click(object sender, RoutedEventArgs e)
        {
            if (txtTableName.Text != "")
            {
                admin.DeleteTable(int.Parse(txtTableID.Text), txtTableName.Text);
                Load_dtgTable();
            }
            else
            {
                MessageBox.Show("Tên bàn trống, bạn không thể xóa", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Xóa bàn ăn không thành công");
            }
        }

        private void btnEditTable_Click(object sender, RoutedEventArgs e)
        {
            if (txtTableName.Text != "" && cbbTableStatus.Text != "" && txtTableLocation.Text != "" && cbbTableType.Text != "")
            {
                admin.EditTable(int.Parse(txtTableID.Text), txtTableName.Text, cbbTableStatus.Text, cbbTableType.Text, txtTableLocation.Text);
                Load_dtgTable();
            }
            else if (txtTableName.Text == "")
            {
                MessageBox.Show("Tên bàn trống, bạn không thể chỉnh sửa", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa bàn không thành công");
            }
            else if (cbbTableStatus.Text == "")
            {
                MessageBox.Show("trạng thái bàn trống, bạn không thể chỉnh sửa", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa bàn không thành công");
            }
            else if (txtTableLocation.Text == "")
            {
                MessageBox.Show("Vị trí bàn trống, bạn không thể chỉnh sửa", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa bàn không thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn loại bàn, bạn không thể chỉnh sửa", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa bàn không thành công");
            }
        }

        private void btnFindTable_Click(object sender, RoutedEventArgs e)
        {
            if (txtFindTable.Text != "")
            {
                List<TblTable> listTable = new List<TblTable>();
                listTable = admin.FindTableByKey(txtFindTable.Text);
                dtgTable.ItemsSource = listTable;
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập từ khóa tìm kiếm", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Tìm kiếm danh sách bàn không thành công");
            }
        }

        private void btnSelectTable_Click(object sender, RoutedEventArgs e)
        {
            Load_dtgTable();
        }
        #endregion

        #region AccountEvent
        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            if (txtAccountName.Text != "" && txtAccountPassword.Text != "" && txtAccountDisplayName.Text != "")
            {
                admin.AddAccount(txtAccountName.Text, txtAccountPassword.Text, txtAccountDisplayName.Text, cbbAccountType.Text);
                Load_dtgAccount();
            }
            else if (txtAccountName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên tài khoản", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới tài khoản không thành công");
            }
            else if (txtAccountPassword.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu tài khoản", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới tài khoản không thành công");
            }
            else if (cbbAccountType.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn loại tài khoản", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới tài khoản không thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tên hiển thị", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Thêm mới tài khoản không thành công");
            }
        }

        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (txtAccountName.Text != "")
            {
                admin.DeleteAccount(int.Parse(txtAccountID.Text), txtAccountName.Text);
                Load_dtgAccount();
            }
            else
            {
                MessageBox.Show("Tên tài khoản không tồn tại. bạn không thể xóa", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Xóa tài khoản không thành công");
            }
        }

        private void btnEditAccount_Click(object sender, RoutedEventArgs e)
        {
            if (txtAccountName.Text != "" && txtAccountPassword.Text != "" && txtAccountDisplayName.Text != "")
            {
                admin.EditAccount(int.Parse(txtAccountID.Text), txtAccountName.Text, txtAccountPassword.Text, txtAccountDisplayName.Text, cbbAccountType.Text);
                Load_dtgAccount();
            }
            else if (txtAccountName.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên tài khoản", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa tài khoản không thành công");
            }
            else if (txtAccountPassword.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu tài khoản", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa tài khoản không thành công");
            }
            else if (cbbAccountType.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập loại tài khoản", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa tài khoản không thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tên hiển thị", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Chỉnh sửa tài khoản không thành công");
            }
        }

        private void btnFindAccount_Click(object sender, RoutedEventArgs e)
        {
            if (txtFindAccount.Text != "")
            {
                List<TblAccount> listAccount = new List<TblAccount>();
                listAccount = admin.FindAccountByKey(txtFindAccount.Text);
                dtgAccount.ItemsSource = listAccount;
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập từ khóa tìm kiếm", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                MessageBox.Show("Tìm kiếm tài khoản không thành công");
            }
        }

        private void btnSelectAccount_Click(object sender, RoutedEventArgs e)
        {
            Load_dtgAccount();
        }
        #endregion

        #region Bill
        private void btnFindBill_Click(object sender, RoutedEventArgs e)
        {
            dtgBill.ItemsSource=admin.GetListBillFromTo(datePickerFrom.SelectedDate.Value, datePickerTo.SelectedDate.Value);
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            TableManager table = new TableManager();
            table.ShowDialog();
        }
    }
}
