using ProductAPI.Daos;
using ProductAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Models
{
    public static class ProductSeed
    {
        public static void InitData(ProductContext context)
        {
            var rnd = new Random();

            var adjectives = new[] { "Small", "Ergonomic", "Rustic",
                "Smart", "Sleek" };
            var materials = new[] { "Steel", "Wooden", "Concrete", "Plastic",
                "Granite", "Rubber" };
            var names = new[] { "Chair", "Car", "Computer", "Pants", "Shoes" };
            var departments = new[] { "Books", "Movies", "Music",
                "Games", "Electronics" };

            var numberOfRelatedProducts = 0;

            context.Products.AddRange(500.Times(x =>
            {
                var adjective = adjectives[rnd.Next(0, 5)];
                var material = materials[rnd.Next(0, 5)];
                var name = names[rnd.Next(0, 5)];
                var department = departments[rnd.Next(0, 5)];
                var productId = $"{x,-3:000}";

                List<RelatedProduct> relatedProducts = new List<RelatedProduct>();

                var hasRelatedProduct = Convert.ToBoolean(rnd.Next(0, 2));
                if (hasRelatedProduct)
                {
                    var numberRelatedProducts = rnd.Next(0, 5);
                    for (int i = 0; i < numberRelatedProducts; i++)
                    {
                        numberOfRelatedProducts += 1;
                        var radjective = adjectives[rnd.Next(0, 5)];
                        var rmaterial = materials[rnd.Next(0, 5)];
                        var rname = names[rnd.Next(0, 5)];
                        var rdepartment = departments[rnd.Next(0, 5)];
                        var rproductId = $"{x + 500 + numberOfRelatedProducts,-3:000}";

                        relatedProducts.Add(new RelatedProduct
                        {
                            ProductNumber =
                                $"{rdepartment.First()}{rname.First()}{rproductId}",
                            Name = $"{radjective} {rmaterial} {rname}",
                            Price = (double)rnd.Next(1000, 9000) / 100,
                            Department = rdepartment
                        });
                    }

                }

                return new Product
                {
                    ProductNumber =
                        $"{department.First()}{name.First()}{productId}",
                    Name = $"{adjective} {material} {name}",
                    Price = (double)rnd.Next(1000, 9000) / 100,
                    Department = department,
                    RelatedProducts = relatedProducts
                };
            }));

            context.SaveChanges();

            var sellers = new[] { "SellsALot", "FormerlyKMart", "WallyWurld", "JupiterJeans", "Howard-Franklin" };
            var id = 1;
            context.Sellers.AddRange(3.Times(s =>
            {
                var seller = sellers[rnd.Next(0, 5)];
                var sellerId = id;
                id++;
                //Builds the Inventory list for the seller
                List<InventoryItem> inventory = new List<InventoryItem>();
                foreach (Product product in context.Products)
                {
                    inventory.Add(new InventoryItem
                    {
                        SellerName = seller,
                        ProductNumber = product.ProductNumber,
                        NumberHeld = 100,
                        NumberSold = 0
                    });
                }
                return new Seller
                {
                    SellerName = seller,
                    SellerId = sellerId.ToString(),
                    ProductsSold = inventory
                };
            }));
            context.SaveChanges();
        }
    }
}
