using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class CategoryFoodDAO : ICategoryFood
    {
        qlcafe db = new qlcafe();
        public void Delete(CategoryFood obj)
        {
            db.CategoryFoods.Attach(obj);
            db.CategoryFoods.Remove(obj);
            db.SaveChanges();
        }

        public List<CategoryFood> GetAll()
        {
            return db.CategoryFoods.ToList();
        }

        public CategoryFood GetById(int id)
        {
            return db.CategoryFoods.Find(id);
        }

        public CategoryFood Insert(CategoryFood obj)
        {
            db.CategoryFoods.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public void Update(CategoryFood obj)
        {
            db.CategoryFoods.Attach(obj);
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
