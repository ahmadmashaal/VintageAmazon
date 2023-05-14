using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;


namespace VintageAmazon.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                objFromDb.CoverTypeId = obj.CoverTypeId;
                objFromDb.Quantity = obj.Quantity;

                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }

        public IEnumerable<Product> Search(string term)
        {
            var result = _db.Products.Include(u => u.Author)
                .Where(x => x.Title.Contains(term)
                || x.Description.Contains(term)
                || x.Author.Contains(term)).ToList();

            return result;
        }

        public IEnumerable<Product> AdvanceSearch(string? title, string? author, string? category, double? minPrice, double? maxPrice)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(p => p.Title.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(p => p.Author.ToLower().Contains(author.ToLower()));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Equals(category.ToLower()));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return query;
        }

        public int SubtractQuantity(Product product, int count)
        {
            product.Quantity -= count;
            return product.Quantity;
        }

        //    public IActionResult AdvanceSearch(string title, int? category, string author, double? minPrice, double? maxPrice)
        //    {
        //        var products = _db.Products.Include(p => p.Category);

        //        if (!string.IsNullOrEmpty(title))
        //        {
        //            products = products.Where(p => p.Title.Contains(title));
        //        }

        //        if (category.HasValue)
        //        {
        //            products = products.Where(p => p.Category.Id == category.Value);
        //        }

        //        if (!string.IsNullOrEmpty(author))
        //        {
        //            products = products.Where(p => p.Author.Contains(author));
        //        }

        //        if (minPrice.HasValue)
        //        {
        //            products = products.Where(p => p.Price >= minPrice.Value);
        //        }

        //        if (maxPrice.HasValue)
        //        {
        //            products = products.Where(p => p.Price <= maxPrice.Value);
        //        }

        //        var result = products.ToList();

        //        return new OkObjectResult(result);
        //    }
    }
}