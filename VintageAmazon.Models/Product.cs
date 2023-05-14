using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageAmazon.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\S]*$")]
        [StringLength(5)]
        [ValidateNever]
        public string? Rating { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,10000)]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Price for 1-50")]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Price for 51-100")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Price for 100+")]
        public double Price100 { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        [ValidateNever]

        public CoverType CoverType { get; set; }

        public int RateCount
        {
            get
            {
                if (ratings != null)
                {
                    return ratings.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int RateTotal
        {
            get
            {
                if(ratings != null)
                {
                    return (ratings.Sum(m => m.Rate));
                }
                else
                {
                    return 0;
                }
            }
        }

        [ValidateNever]
        public virtual ICollection<StarRating> ratings { get; set; }
    }
}
