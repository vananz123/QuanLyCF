using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class BillInfoDAO : IBillInfo
    {
        qlcafe db = new qlcafe();
        public void Delete(BillInfo obj)
        {
            db.BillInfoes.Attach(obj);
            db.BillInfoes.Remove(obj);
            db.SaveChanges();
        }

        public List<BillInfo> GetAll()
        {
            return db.BillInfoes.ToList();
        }

        public BillInfo GetById(int id)
        {
            throw new NotImplementedException();
        }

        public BillInfo Insert(BillInfo obj)
        {
            db.BillInfoes.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public void Update(BillInfo obj)
        {
            db.BillInfoes.Attach(obj);
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
