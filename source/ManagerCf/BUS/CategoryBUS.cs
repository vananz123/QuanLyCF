using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;

namespace BUS
{
    public static class CategoryBUS
    {
        static ICategoryFood icategoryFood;
        static CategoryBUS()
        {
            icategoryFood = new CategoryFoodDAO();
        }
        public static List<CategoryFood> GetAll()
        {
            return icategoryFood.GetAll();
        }
        public static CategoryFood GetById(int id)
        {
            return icategoryFood.GetById(id);
        }
        public static CategoryFood Insert(CategoryFood obj)
        {
            return icategoryFood.Insert(obj);
        }
        public static void Delete(CategoryFood obj)
        {
            icategoryFood.Delete(obj);
        }
        public static void Update(CategoryFood obj)
        {
            icategoryFood.Update(obj);
        }
        
    }

}
