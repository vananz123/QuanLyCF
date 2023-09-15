using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class FoodDAO : IFood
    {
        qlcafe db = new qlcafe();
        public void Delete(Food obj)
        {
            db.Foods.Attach(obj);
            db.Foods.Remove(obj);
            db.SaveChanges();
        }

        public List<Food> GetAll()
        {
            var f = db.Foods.Select(p => p);
            return f.ToList();
        }

        public Food GetById(int id)
        {
            return db.Foods.Find(id);
        }

        public Food Insert(Food obj)
        {
            db.Foods.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public void Update(Food obj)
        {
            db.Foods.Attach(obj);
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
