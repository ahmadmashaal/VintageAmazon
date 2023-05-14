using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;

namespace VintageAmazon.DataAccess.Repository
{
    public class StarRatingRepository : Repository<StarRating>, IStarRatingRepository
    {
        private ApplicationDbContext _db;

        public StarRatingRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(StarRating obj)
        {
            _db.StarsRatings.Update(obj);
        }
    }
}
