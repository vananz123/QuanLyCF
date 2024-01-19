using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public interface IAccount
    {
        List<Account> GetAll();
        Account GetByUserName(string username);
        Account Insert(Account obj);
        void Update(Account obj);
        void Delete(Account obj);

    }
}
