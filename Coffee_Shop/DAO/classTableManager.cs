using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee_Shop.DAO
{
    class classTableManager
    {
        Coffee_ShopEntities data = new Coffee_ShopEntities();
        #region Food
        // Lấy ID từ tên món ăn
        public int GetIDByFoodName(string FoodName)
        {
            return data.TblFoods.First(n => n.Name == FoodName).ID;
        }
        #endregion

        #region Category
        public int GetCategoryIDByName(string categoryName)
        {
            return data.TblFoodCategories.First(n => n.Name == categoryName).ID;
        }
        #endregion

        #region Table
        List<TblTable> listTable = new List<TblTable>();
        public List<TblTable> GetTable()
        {
            listTable = data.TblTables.ToList();
            return listTable;
        }

        // Chỉnh sửa trạng thái bàn ăn
        public void EditStatusTable(int TableID, string Status)
        {
            TblTable editTable = new TblTable();
            editTable = data.TblTables.Single(n => n.ID == TableID);
            editTable.TableStatus = Status;
            data.SaveChanges();
        }

        public int GetTableIDByTableName(string TableName)
        {
            return data.TblTables.First(n => n.Name == TableName).ID;
        }

        // kiểm tra trạng thái hiện tại của bàn
        public string StatusOfTable(int TableID)
        {
            return data.TblTables.Single(n => n.ID == TableID).TableStatus;
        }

        public bool IsTableCheckOut(int TableID)
        {
            if(data.TblBills.Where(n=>n.TableID==TableID&&n.BillStatus=="Chưa thanh toán").Count()>0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Account
        #endregion

        #region BillInfor
        // Tìm xem món ăn đã tồn tại trong Bill chưa (input: IDBill+FoodID output:true/false)
        public bool IsFoodExistInBillInfor(int FoodID, int BillID)
        {
            if (data.TblBillInfors.Where(n => n.BillID == BillID && n.FoodID == FoodID).Count() == 0)
            {
                return false;
            }
            return true;
        }

        // Nếu món đã tồn tại thì lấy ra số lượng (input: IDBill+FoodID output: Quantity)
        public int GetFoodQuantity(int FoodID, int BillID)
        {
            return data.TblBillInfors.Single(n => n.FoodID == FoodID && n.BillID == BillID).CountF;
        }

        // Nếu món tồn tại rồi thì chỉnh sửa BillInfor(new Quantity)
        public void EditNewQuantityBillInfor(int QuantityAdd, int FoodID, int BillID)
        {
            TblBillInfor newBillInfor = new TblBillInfor();
            newBillInfor = data.TblBillInfors.Single(n => n.BillID == BillID && n.FoodID == FoodID);
            newBillInfor.CountF = GetFoodQuantity(FoodID, BillID) + QuantityAdd;
            newBillInfor.Total = (GetFoodQuantity(FoodID, BillID) + QuantityAdd) * data.TblFoods.First(n => n.ID == FoodID).Price;
            data.SaveChanges();
        }

        public void EditBillInfor(int Quantity, int FoodID, int BillID)
        {
            TblBillInfor newBillInfor = new TblBillInfor();
            newBillInfor = data.TblBillInfors.Single(n => n.BillID == BillID && n.FoodID == FoodID);
            newBillInfor.CountF = Quantity;
            newBillInfor.Total = Quantity * data.TblFoods.First(n => n.ID == FoodID).Price;
            data.SaveChanges();
        }

        // Nếu món ấy chưa tồn tại thì tạo BillInfor mới (input: IDBill+FoodID output: null)
        public void AddBillInfor(int FoodID, int BillID, int Quantity)
        {
            TblBillInfor newBillInfor = new TblBillInfor()
            {
                BillID = BillID,
                FoodID = FoodID,
                CountF = Quantity,
                Total = Quantity * data.TblFoods.First(n => n.ID == FoodID).Price
            };
            data.TblBillInfors.Add(newBillInfor);
            data.SaveChanges();
        }

        // Xóa BillInfor nếu quantity=0
        public void DeleteBillInfor(int FoodID, int BillID)
        {
            data.TblBillInfors.Remove(data.TblBillInfors.Single(n => n.BillID == BillID && n.FoodID == FoodID));
            data.SaveChanges();
        }

        // Lấy ra tổng tiền từng billInfor (input:TableID+BillID output:sum())
        public double TotalBill(int BillID)
        {
            return data.TblBillInfors.Where(n => n.BillID == BillID).ToList().Sum(n => n.Total);
        }

        public bool IsBillInforExists(int BillID)
        {
            if (data.TblBillInfors.Where(n => n.BillID == BillID).Count() == 0)
            {
                return false;
            }
            return true;
        }
        // Lấy ra danh sách BillInfor (input: TableID+BillID, output:ListBillInfor(FoodName))
        public IEnumerable<object> GetListBillInforWithFoodName(int BillID)
        {
            List<TblBillInfor> listBillInfor = new List<TblBillInfor>();
            var result = data.TblBillInfors.Where(n => n.BillID == BillID).ToList().Join(data.TblFoods.ToList(), x => x.FoodID, y => y.ID, (x, y) => new
            {
                BillID = x.BillID,
                FoodName = y.Name,
                Price = y.Price,
                Quantity = x.CountF,
                Total = x.Total,
                FoodID = x.FoodID,
                CategoryID=y.CategoryID
            });
            var result2 = result.Join(data.TblFoodCategories.ToList(), x => x.CategoryID, y => y.ID, (x, y) => new
            {
                BillID = x.BillID,
                FoodName = x.FoodName,
                CategoryName = y.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                Total = x.Total
            });
            return result2;
        }
        #endregion

        #region Bill
        // Kiểm tra Bill đã tồn tại chưa ( input là TableID condition:"chưa thanh toán")
        public bool IsBillExists(int TableID)
        {
            TblBill bill = new TblBill();
            if (data.TblBills.Where(n => n.TableID == TableID && n.BillStatus == "Chưa thanh toán").Count() == 0)
            {
                return false; // chưa tồn tại
            }
            else
                return true;
        }

        // Nếu Bill đã tồn tại thì trả về IDBill (input là TableID condition:"chưa thanh toán")
        public int GetBillIDByTableID(int TableID)
        {
            if (IsBillExists(TableID))
            {
                return data.TblBills.Single(n => n.TableID == TableID && n.BillStatus == "Chưa thanh toán").ID;
            }
            else return -1;

        }

        // Nếu Bill chưa tồn tại thì tạo mới Bill. Sau đó chỉnh sửa ở bảng TblTable (change Status từ "trống" sang "có khách")
        public void AddNewBill(int TableID, string Status, DateTime TimeCheckIn, DateTime? TimCheckOut, double TotalBill)
        {
            TblBill newBill = new TblBill()
            {
                TableID = TableID,
                BillStatus = Status,
                DateCheckIn = TimeCheckIn,
                DateCheckOut = TimCheckOut,
                TotalBill = TotalBill
            };
            data.TblBills.Add(newBill);
            data.SaveChanges();
        }

        // Sau khi nhấn thanh toán EditBill (thêm thời gian checkout, đổi "chưa thanh toán" sang "đã thanh toán"). Sau đó chỉnh sửa ở bảng TblTable (change Status từ "trống" sang "có khách")
        public void EditBill(int BillID, int TableID, string Status, DateTime? TimCheckOut, double TotalBill)
        {
            TblBill bill = new TblBill();
            bill = data.TblBills.Single(n => n.ID == BillID);
            bill.TableID = TableID;
            bill.BillStatus = Status;
            bill.DateCheckOut = TimCheckOut;
            bill.TotalBill = TotalBill;
            data.SaveChanges();
        }
        #endregion

        #region Load_Combobox
        List<string> foodCombobox = new List<string>();
        List<string> categorycombobox = new List<string>();
        public List<string> LoadFoodCombobox()
        {
            foodCombobox = data.TblFoods.Select(n => n.Name).ToList();
            return foodCombobox;
        }

        public List<string> LoadcategoryCombobox()
        {
            categorycombobox = data.TblFoodCategories.Select(n => n.Name).ToList();
            return categorycombobox;
        }
        public List<string> GetFoodComboboxByID(int ID)
        {
            foodCombobox = data.TblFoods.Where(n => n.CategoryID == ID).Select(n => n.Name).ToList();
            return foodCombobox;
        }
        #endregion
    }
}
