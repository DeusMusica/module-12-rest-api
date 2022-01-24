using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Daos
{

    public class SellerDao : DbContext
    {
        public SellerDao(DbContextOptions<SellerDao> options) : base(options)
        {
        }

        public DbSet<Seller> Sellers { get; set; }
    }

}
