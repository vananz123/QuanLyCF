using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;

namespace BUS
{
    public static class AccountBUS
    {
        static IAccount iaccount;
        static Account account;
        static AccountBUS()
        {
            iaccount = new AccountDAO();
        }
        public static void SetAccount(Account acc)
        {
            account = acc;
        }
        public static Account GetAccount()
        {
            return account;
        }
        public static Account CheckLogin(string user,string pw)
        {
            Account acc=null;
            foreach(var a in AccountBUS.GetALL())
            {
                if(a.UserName == user && a.Password == pw)
                {
                    acc = a;
                }
            }
            return acc;
        }
        public static List<Account> GetALL()
        {
            return iaccount.GetAll();
        }
        public static Account GetByUserName(string username)
        {
            return iaccount.GetByUserName(username);
        }
        public static Account Insert(Account obj)
        {
            return iaccount.Insert(obj);
        }
        public static void Update(Account obj)
        {
            iaccount.Update(obj);
        }
        public static void Delete(Account obj)
        {
            iaccount.Delete(obj);
        }

    }
}
