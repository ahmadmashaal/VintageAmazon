using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageAmazon.Models;

namespace VintageAmazon.DataAccess.Repository.IRepository
{
    public interface IStarRatingRepository : IRepository<StarRating>
    {
        void Update(StarRating obj);
    }
}
