using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;

namespace VintageAmazon.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _dbb;

        public CategoryRepository(ApplicationDbContext db) : base(db) 
        {
            _dbb = db;
        }

        public void Update(Category obj)
        {
            _dbb.Categories.Update(obj);
        }
        public IEnumerable<Category> OrderBy(Func<Category, string> keySelector)
        {
            return _dbb.Categories.OrderBy(keySelector);
        }

    }
}
