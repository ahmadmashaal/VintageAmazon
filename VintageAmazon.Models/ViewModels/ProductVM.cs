using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Impleminting Tightly binded view

namespace VintageAmazon.Models.ViewModels
{
    public class ProductVM
    {
        
        public Product Product { get; set; }
        //public IEnumerable<Comment> Comment { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    
    }
}

