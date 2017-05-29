using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Coffee_Shop.DAO
{
    class Admin
    {
        Coffee_ShopEntities data = new Coffee_ShopEntities();
        #region Food
        List<TblFood> listFood = new List<TblFood>();

        // Get danh sách món ăn
        public List<TblFood> getListFood()
        {
            listFood = data.TblFoods.ToList();
            return listFood;
        }

        // Thêm món ăn
        public void AddFood(string Name,int IDcategory,float Price)
        {
            TblFood food = new TblFood()
            {
                Name=Name,CategoryID=IDcategory,Price=Price
            };
            data.TblFoods.Add(food);
            data.SaveChanges();
        }

        // Xóa món ăn
        public void DeleteFood(int ID,string Name)
        {
            try
            {
                data.TblFoods.Remove(data.TblFoods.Single(n => n.ID == ID&&n.Name==Name));
                data.SaveChanges();
            }
            catch(Exception e)
            {
                MessageBox.Show("Món ăn không có trong danh sách");
            }
        }

        // Sửa món ăn
        public void EditFood(int ID,string Name,int CategoryID,float Price)
        {
            TblFood food = new TblFood();
            food=data.TblFoods.Single(n => n.ID == ID);
            food.Name = Name;
            food.CategoryID = CategoryID;
            food.Price = Price;
            data.SaveChanges();
        }

        // Tìm kiếm món ăn theo tên
        public List<TblFood> FindFoodByKey( string key)
        {
            List<TblFood> listFood = new List<TblFood>();
            listFood = data.TblFoods.Where(n => n.Name.Contains(key)).ToList();
            return listFood;
        }

        public IEnumerable<object> GetListFoodWithCategoryName()
        {
            var result = getListFood().Join(getListCategory(), x => x.CategoryID, y => y.ID, (x, y) => new
                {
                    ID=x.ID,Name=x.Name,CategoryName=y.Name,Price=x.Price
                });
            return result;
        }

        public IEnumerable<object> FindFoodByKeyWithCategoryName(string Key)
        {
            var result = FindFoodByKey(Key).Join(getListCategory(), x => x.CategoryID, y => y.ID, (x, y) => new
                {
                    ID = x.ID,
                    Name = x.Name,
                    CategoryName = y.Name,
                    Price = x.Price
                });
            return result;
        }
        #endregion

        #region Category
        List<TblFoodCategory> listCategory = new List<TblFoodCategory>();
        public List<TblFoodCategory> getListCategory()
        {
            listCategory = data.TblFoodCategories.ToList();
            return listCategory;
        }

        public int GetCategoryIDByName(string Name)
        {
            return data.TblFoodCategories.First(n => n.Name == Name).ID;
        }

        // Thêm danh mục
        public void AddCategory(string Name)
        {
            TblFoodCategory category = new TblFoodCategory()
            {
                Name = Name
            };
            data.TblFoodCategories.Add(category);
            data.SaveChanges();
        }

        // Xóa danh mục
        public void DeleteCategory(int ID,string Name)
        {
            try
            {
                data.TblFoodCategories.Remove(data.TblFoodCategories.Single(n => n.ID == ID&&n.Name==Name));
                data.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Danh mục không có trong danh sách");
            }
        }

        // Sửa danh mục
        public void EditCategory(int ID, string Name)
        {
            TblFoodCategory category = new TblFoodCategory();
            category = data.TblFoodCategories.Single(n => n.ID == ID);
            category.Name = Name;
            data.SaveChanges();
        }

        // Tìm kiếm danh mục theo tên
        public List<TblFoodCategory> FindCategoryByKey(string key)
        {
            List<TblFoodCategory> listCategory = new List<TblFoodCategory>();
            listCategory = data.TblFoodCategories.Where(n => n.Name.Contains(key)).ToList();
            return listCategory;
        }
        #endregion

        #region Table
        List<TblTable> listTable = new List<TblTable>();
        public List<TblTable> getListTable()
        {
            listTable = data.TblTables.ToList();
            return listTable;
        }

        // Thêm bàn
        public void AddTable(string Name,string Status,string TableType, string Location)
        {
            TblTable table = new TblTable()
            {
                Name = Name,
                TableStatus = Status,
                TableType=TableType,
                Location=Location
            };
            data.TblTables.Add(table);
            data.SaveChanges();
        }

        // Xóa bàn
        public void DeleteTable(int ID,string Name)
        {
            try
            {
                data.TblTables.Remove(data.TblTables.Single(n => n.ID == ID&&n.Name==Name));
                data.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Bàn không có trong danh sách");
            }
        }

        // Sửa bàn
        public void EditTable(int ID, string Name,string Status, string TableType, string Location)
        {
            TblTable table = new TblTable();
            table = data.TblTables.Single(n => n.ID == ID);
            table.Name = Name;
            table.TableStatus = Status;
            table.Location = Location;
            table.TableType = TableType;
            data.SaveChanges();
        }

        // Tìm kiếm bàn theo tên
        public List<TblTable> FindTableByKey(string key)
        {
            List<TblTable> listTable = new List<TblTable>();
            listTable = data.TblTables.Where(n => n.Name.Contains(key)).ToList();
            return listTable;
        }
        #endregion

        #region Account
        List<TblAccount> listAccount = new List<TblAccount>();
        public List<TblAccount> getListAccount()
        {
            listAccount = data.TblAccounts.ToList();
            return listAccount;
        }

        // Thêm Account
        public void AddAccount(string UserName, string PassWord, String DisplayName,string Type)
        {
            TblAccount account = new TblAccount()
            {
                UserName = UserName,
                Pass = PassWord,
                DisplayName = DisplayName,
                AccountType=Type
            };
            data.TblAccounts.Add(account);
            data.SaveChanges();
        }

        // Xóa Account
        public void DeleteAccount(int ID,string UserName)
        {
            try
            {
                data.TblAccounts.Remove(data.TblAccounts.Single(n => n.ID == ID&&n.UserName==UserName));
                data.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Món ăn không có trong danh sách");
            }
        }

        // Sửa món ăn
        public void EditAccount(int ID, string UserName, string PassWord, string DisplayName,string Type)
        {
            TblAccount account = new TblAccount();
            account = data.TblAccounts.Single(n => n.ID == ID);
            account.UserName = UserName;
            account.Pass = PassWord;
            account.DisplayName = DisplayName;
            account.AccountType = Type;
            data.SaveChanges();
        }

        // Tìm kiếm món ăn theo tên
        public List<TblAccount> FindAccountByKey(string key)
        {
            List<TblAccount> listAccount = new List<TblAccount>();
            listAccount = data.TblAccounts.Where(n => n.UserName.Contains(key)).ToList();
            return listAccount;
        }
        #endregion

        #region Bill
        public List<TblBill> GetListBillFromTo(DateTime From,DateTime To)
        {
            List<TblBill> listBillFromTo = new List<TblBill>();
            try
            {
                listBillFromTo = data.TblBills.Where(n => n.DateCheckOut <= To && n.DateCheckOut >= From).ToList();
            }
            catch(Exception e)
            {
                MessageBox.Show("Không có hóa đơn nào trong khoảng thời gian được chọn");
            }
            return listBillFromTo;
        }
        public List<TblBill> GetListBill()
        {
            List<TblBill> allListBill = new List<TblBill>();
            allListBill = data.TblBills.ToList();
            return allListBill;
        }
        #endregion
    }
}
