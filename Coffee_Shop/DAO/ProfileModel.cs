using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee_Shop.DAO
{
    class ProfileModel
    {
        Coffee_ShopEntities data = new Coffee_ShopEntities();
        public void EditAccount(int ID,string DisplayName,string NewPassWord)
        {
            TblAccount account = new TblAccount();
            account = data.TblAccounts.Single(n => n.ID == ID);
            account.DisplayName = DisplayName;
            account.Pass = NewPassWord;
            data.SaveChanges();
        }
        public bool IsPasswordTrue(int ID,string Password)
        {
            if(data.TblAccounts.Where(n=>n.ID==ID && n.Pass==Password).Count()==0)
            {
                return false;
            }
            return true;
        }
    }
}
