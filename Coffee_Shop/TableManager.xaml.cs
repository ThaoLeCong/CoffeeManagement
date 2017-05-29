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
using System.IO;

namespace Coffee_Shop
{
    /// <summary>
    /// Interaction logic for TableManager.xaml
    /// </summary>
    public partial class TableManager : Window
    {
        classTableManager clTableManager = new classTableManager();
        TblAccount account = new TblAccount();
        private int numberic;
        private int numberic1;
        public TableManager()
        {
            InitializeComponent();
            Numberic = 0;
            Numberic1 = 0;
            cbbCategory.ItemsSource = clTableManager.LoadcategoryCombobox();
            cbbFood.ItemsSource = clTableManager.LoadFoodCombobox();
            cbbChangeTable.ItemsSource = clTableManager.GetTable().Select(x => x.Name);
            LoadTable();
            dtgBillInfor.IsReadOnly = true;
        }
        // taoj constructor truyền dữ liệu giữa 2 windown
        public TableManager(TblAccount loginAccount)
            : this()
        {
            account = loginAccount;
            if (account.AccountType != "admin")
            {
                menuAdmin.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        #region Table
        private void LoadTable()
        {
            // hiển thị danh sách bàn ăn
            List<TblTable> listTable = new List<TblTable>();
            listTable = clTableManager.GetTable();
            foreach (var item in listTable)
            {
                Button btn = new Button()
                {
                    Width = 120,
                    Height = 90,
                };
                switch (item.TableStatus)
                {
                    case "Trống":
                        btn.Background = Brushes.Aqua;
                        break;
                    case "Đã đặt":
                        btn.Background = Brushes.Green;
                        break;
                    case "Có khách":
                        btn.Background = Brushes.Coral;
                        break;
                }
                btn.Content = item.Name + Environment.NewLine + item.TableStatus;
                btn.Margin = new System.Windows.Thickness(20, 10, 10, 10);
                btn.Click += btn_Click;
                btn.Tag = item.ID;
                wrapRectange.Children.Add(btn);
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            // Load BillInfor
            int TableID = int.Parse((sender as Button).Tag.ToString());
            dtgBillInfor.Tag = TableID;
            Load_dtgBillInfor(TableID);
        }

        private void RemoveButton()
        {
            List<TblTable> listTable = new List<TblTable>();
            listTable = clTableManager.GetTable();
            wrapRectange.Children.RemoveRange(0, listTable.Count());
        }

        private void btnChangeTable_Click(object sender, RoutedEventArgs e)
        {
            if (cbbChangeTable.SelectedValue != null)
            {
                int TableID = clTableManager.GetTableIDByTableName(cbbChangeTable.SelectedValue.ToString());
                // kiểm tra trạng thái bàn cần chuyển đến
                if (clTableManager.StatusOfTable(TableID) == "Trống")
                {
                    int BillID = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString()));
                    clTableManager.EditBill(BillID, TableID, "Chưa thanh toán", null, 0);
                    clTableManager.EditStatusTable(TableID, "Có khách");
                    clTableManager.EditStatusTable(int.Parse(dtgBillInfor.Tag.ToString()), "Trống");
                    dtgBillInfor.Tag = TableID;
                    RemoveButton();
                    LoadTable();
                    MessageBox.Show("Chuyển bàn thành công");
                }
                else
                {
                    MessageBox.Show("Bạn không thể chuyển vì bàn đã đặt hoặc có khách");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn bàn cần chuyển");
            }
        }
        #endregion

        #region BillInfor
        private void btnAddFood_Click(object sender, RoutedEventArgs e)
        {
            if (cbbFood.SelectedValue != null)
            {
                int FoodID = clTableManager.GetIDByFoodName(cbbFood.SelectedValue.ToString());
                // check bill của bàn đấy đã tồn tại chưa
                if (clTableManager.IsBillExists(int.Parse(dtgBillInfor.Tag.ToString())))
                {
                    int BillID = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString()));
                    // check món ăn cần thêm đã tồn tại trong bill chưa
                    if (clTableManager.IsFoodExistInBillInfor(FoodID, BillID))
                    {
                        clTableManager.EditNewQuantityBillInfor(int.Parse(txtNumeric.Text), FoodID, BillID);
                    }
                    else // chưa tồn tại thêm mới BillInfor
                    {
                        clTableManager.AddBillInfor(FoodID, BillID, int.Parse(txtNumeric.Text));
                    }
                }
                else // add new bill
                {
                    clTableManager.AddNewBill(int.Parse(dtgBillInfor.Tag.ToString()), "Chưa thanh toán", DateTime.Now, null, 0);
                    int BillID = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString()));
                    clTableManager.AddBillInfor(FoodID, BillID, int.Parse(txtNumeric.Text));
                    // thay đổi trạng thái bàn
                    clTableManager.EditStatusTable(int.Parse(dtgBillInfor.Tag.ToString()), "Có khách");
                }
                // Load dtgBillInfor
                Load_dtgBillInfor(int.Parse(dtgBillInfor.Tag.ToString()));
                RemoveButton();
                // Load Table
                LoadTable();
            }
            else if (cbbFood.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Bạn chưa chọn tên món ăn. Thêm món ăn không thành công");
            }
        }

        private void btnEditFood_Click(object sender, RoutedEventArgs e)
        {
            var infor = dtgBillInfor.SelectedCells[1];
            TextBlock x = infor.Column.GetCellContent(dtgBillInfor.SelectedItem) as TextBlock;
            int BillID = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString()));
            string FoodNameSelectedDtg = x.Text;
            if (txtNumeric.Text == "" && cbbFood.SelectedValue.ToString() == FoodNameSelectedDtg)
            {
                // delete this BillInfor
                clTableManager.DeleteBillInfor(clTableManager.GetIDByFoodName(FoodNameSelectedDtg), BillID);
            }
            else if (txtNumeric.Text != "" && cbbFood.SelectedValue.ToString() == FoodNameSelectedDtg)
            {
                clTableManager.EditBillInfor(int.Parse(txtNumeric.Text), clTableManager.GetIDByFoodName(FoodNameSelectedDtg), BillID);
            }
            else
            {

            }
            cbbFood.SelectedValue = null;
            cbbFood.ItemsSource = clTableManager.LoadFoodCombobox();
            Load_dtgBillInfor(int.Parse(dtgBillInfor.Tag.ToString()));
        }

        private void btnDeleteFood_Click(object sender, RoutedEventArgs e)
        {
            var infor = dtgBillInfor.SelectedCells[1];
            TextBlock x = infor.Column.GetCellContent(dtgBillInfor.SelectedItem) as TextBlock;
            int BillID = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString()));
            string FoodNameSelectedDtg = x.Text;
            clTableManager.DeleteBillInfor(clTableManager.GetIDByFoodName(FoodNameSelectedDtg), BillID);
            cbbFood.SelectedValue = null;
            cbbFood.ItemsSource = clTableManager.LoadFoodCombobox();
            Load_dtgBillInfor(int.Parse(dtgBillInfor.Tag.ToString()));
        }

        private void Load_dtgBillInfor(int TableID)
        {
            int BillID = clTableManager.GetBillIDByTableID(TableID);
            if (BillID != -1)
            {
                if (clTableManager.IsBillInforExists(BillID))
                {
                    dtgBillInfor.ItemsSource = clTableManager.GetListBillInforWithFoodName(BillID);
                }
            }
            else
            {
                dtgBillInfor.ItemsSource = null;
            }
        }

        private void dtgBillInfor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgBillInfor.SelectedValue != null)
            {
                var infor = dtgBillInfor.SelectedCells[1];
                TextBlock x = infor.Column.GetCellContent(dtgBillInfor.SelectedItem) as TextBlock;
                var infor1 = dtgBillInfor.SelectedCells[3];
                TextBlock x1 = infor1.Column.GetCellContent(dtgBillInfor.SelectedItem) as TextBlock;
                cbbCategory.ItemsSource = clTableManager.LoadcategoryCombobox();
                cbbFood.ItemsSource = clTableManager.GetFoodComboboxByID(clTableManager.GetCategoryIDByName(cbbCategory.Text));
                cbbFood.SelectedValue = x.Text;
                txtNumeric.Text = x1.Text;
            }
        }

        private void ExportToExcel()
        {
            dtgBillInfor.SelectAllCells();
            dtgBillInfor.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dtgBillInfor);
            string resultAt = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            string result = (string)Clipboard.GetData(DataFormats.UnicodeText);
            dtgBillInfor.UnselectAllCells();
            string filename = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString())).ToString();
            System.IO.StreamWriter str = new System.IO.StreamWriter(new FileStream(@"C:\Users\PAC\Documents\BillInfor\" + filename + ".xls", FileMode.Create, FileAccess.ReadWrite), Encoding.Unicode);
            str.WriteLine(result.Replace(",", " "));
            str.Close();
        }

        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            if (dtgBillInfor.Tag != null)
            {
                if (clTableManager.IsTableCheckOut(int.Parse(dtgBillInfor.Tag.ToString())))
                {
                    int BillID = clTableManager.GetBillIDByTableID(int.Parse(dtgBillInfor.Tag.ToString()));
                    double Total = clTableManager.TotalBill(BillID);
                    ExportToExcel();
                    clTableManager.EditBill(BillID, int.Parse(dtgBillInfor.Tag.ToString()), "Đã thanh toán", DateTime.Now, Total);
                    clTableManager.EditStatusTable(int.Parse(dtgBillInfor.Tag.ToString()), "Trống");
                    RemoveButton();
                    LoadTable();
                    Load_dtgBillInfor(int.Parse(dtgBillInfor.Tag.ToString()));
                    MessageBox.Show("Thanh toán thành công");
                }
                else
                {
                    MessageBox.Show("Bàn bạn chọn đã được thanh toán");
                }
            }
            else
                MessageBox.Show("Bạn chưa chọn bàn cần thanh toán");
        }
        #endregion

        #region Numeric_UpDown
        public int Numberic1
        {
            get { return numberic1; }
            set { numberic1 = value; txtNumeric1.Text = numberic1.ToString(); }
        }
        public int Numberic
        {
            get { return numberic; }
            set { numberic = value; txtNumeric.Text = numberic.ToString(); }
        }
        private void txtNumeric_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txtNumeric.Text, out numberic))
            {
                return;
            }
            txtNumeric.Text = numberic.ToString();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            Numberic++;
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            Numberic--;
            if (Numberic < 0)
            {
                Numberic = 0;
            }
        }

        private void txtNumeric1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txtNumeric1.Text, out numberic1))
            {
                return;
            }
            txtNumeric1.Text = numberic1.ToString();
        }

        private void btnUp1_Click(object sender, RoutedEventArgs e)
        {
            Numberic1++;
        }

        private void btnDown1_Click(object sender, RoutedEventArgs e)
        {
            Numberic1--;
            if (Numberic1 < 0)
            {
                Numberic1 = 0;
            }
        }
        #endregion

        #region MenuEvent
        private void menuLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.ShowDialog();
        }

        private void muneProfile_Click(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile(account);
            this.Hide();
            profile.ShowDialog();
            this.ShowDialog();
        }
        #endregion

        #region Combobox_Event
        private void cbbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbCategory.SelectedValue != null)
            {
                cbbFood.ItemsSource = clTableManager.GetFoodComboboxByID(clTableManager.GetCategoryIDByName(cbbCategory.SelectedValue.ToString()));
            }
        }
        #endregion}

        private void btnDisscount_Click(object sender, RoutedEventArgs e)
        {
            // khi xuất hóa đơn sẽ trừ disccount
        }
    }
}
