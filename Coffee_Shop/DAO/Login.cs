using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffee_Shop.DAO
{
    public class Login
    {
        Coffee_ShopEntities data = new Coffee_ShopEntities();
        public bool LoginFuntion(string userName,string passWord)
        {
            List<TblAccount> account = new List<TblAccount>();
            account = data.TblAccounts.Where(x => x.UserName == userName && x.Pass == passWord).ToList();
            if(account.Count()>0)
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
        public TblAccount accountInfor(string userName,string passWord)
        {
            TblAccount account = new TblAccount();
            account = data.TblAccounts.First(x => x.UserName == userName && x.Pass == passWord);
            return account;
        }
    }
}
