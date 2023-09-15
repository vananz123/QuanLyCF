using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class BillDAO : IBill
    {
        qlcafe db = new qlcafe();
        public void Delete(Bill obj)
        {
            db.Bills.Attach(obj);
            db.Bills.Remove(obj);
            db.SaveChanges();
        }

        public List<Bill> GetAll()
        {
            return db.Bills.ToList();
        }

        public Bill GetById(int id)
        {
            return db.Bills.Find(id);
        }

        public Bill Insert(Bill obj)
        {
            db.Bills.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public void Update(Bill obj)
        {
            db.Bills.Attach(obj);
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
