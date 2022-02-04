using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Models
{
    public class SellerPatch
    {
        [Key]        
        [Display(Name = "Seller ID")]
        public string SellerId { get; set; }

        [Display(Name = "Seller's Name")]
        public string SellerName { get; set; }

        [Display(Name = "Products Sold")]
        public virtual List<InventoryItem> ProductsSold { get; set; }

    //    [Display(Name = "Available Stock")]
    //    public int? Stock { get; set; }
    }
}
