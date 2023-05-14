using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageAmazon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace VintageAmazon.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        //void AddReview(Product obj);
        public IEnumerable<Product> Search(string term);
        //public IActionResult AdvanceSearch(string title, int? category, string author, double? minPrice, double? maxPrice);
        public int SubtractQuantity(Product product, int count);
        public IEnumerable<Product> AdvanceSearch(string? title, string? author, string? category, double? minPrice, double? maxPrice);
    }
}
