using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class AccountDAO : IAccount
    {
        qlcafe db = new qlcafe();
        public void Delete(Account obj)
        {
            db.Accounts.Attach(obj);
            db.Accounts.Remove(obj);
            db.SaveChanges();
        }


        public List<Account> GetAll()
        {
            var acc = from a in db.Accounts
                      select a;
            return acc.ToList();
        }

        public Account GetByUserName(string username)
        {
            return db.Accounts.Find(username);
        }

        public Account Insert(Account obj)
        {
            db.Accounts.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public void Update(Account obj)
        {
            db.Accounts.Attach(obj);
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
