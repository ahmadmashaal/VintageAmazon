using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;

namespace VintageAmazon.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }


    }
}
