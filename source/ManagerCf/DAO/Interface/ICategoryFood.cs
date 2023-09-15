using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public interface ICategoryFood
    {
        void Delete(CategoryFood obj);
        List<CategoryFood> GetAll();
        CategoryFood GetById(int id);
        CategoryFood Insert(CategoryFood obj);
        void Update(CategoryFood obj);

    }
}
