using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Daos;
using ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class SellerController : ControllerBase
    {
        private readonly SellerDao _sellerDao;

        public SellerController(SellerDao sellerDao)
        {
            _sellerDao = sellerDao;

            if (_sellerDao.Sellers.Any()) return;
            //ProductSeed.InitData(seller);
        }

       
    [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seller> PostSeller([FromBody] Seller seller)
        {

            try
            {
                _sellerDao.Sellers.Add(seller);
                _sellerDao.SaveChanges();

                return new CreatedResult($"/seller/{seller.SellerId.ToLower()}", seller);
            }
            catch (Exception e)
            {
                return ValidationProblem(e.Message);
            }

        }
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IQueryable<Seller>> GetSeller([FromQuery] string seller)
        {
            var result = _sellerDao.Sellers as IQueryable<Seller>;

            if (!string.IsNullOrEmpty(seller))
            {
                result = result.Where(p => p.SellerName.Equals(seller, StringComparison.InvariantCultureIgnoreCase));
            }

            return Ok(result.OrderBy(p => p.SellerId).Take(15));
        }


        //[HttpPatch]
        //[Route("{Id}")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //copy over PatchProduct information and add seller info so you can change seller info
        //public ActionResult<Product> PatchProduct([FromRoute] string productNumber, [FromBody] ProductPatch newProduct)
        //{
        //    try
        //    {
        //        var productList = _context.Products as IQueryable<Product>;
        //        var product = productList.First(p => p.ProductNumber.Equals(productNumber));

        //        product.ProductNumber = newProduct.ProductNumber ?? product.ProductNumber;
        //        product.Department = newProduct.Department ?? product.Department;
        //        product.Name = newProduct.Name ?? product.Name;
        //        product.Price = newProduct.Price ?? product.Price;
        //        product.RelatedProducts = newProduct.RelatedProducts ?? product.RelatedProducts;
        //        product.LastModified = DateTime.Now;

        //        _context.Products.Update(product);
        //        _context.SaveChanges();

        //        return new CreatedResult($"/products/{product.ProductNumber.ToLower()}", product);
        //    }
        //    catch (Exception e)
        //    {
        //        // Typically an error log is produced here
        //        return ValidationProblem(e.Message);
        //    }
        //}

        [HttpDelete]
        [Route("{SellerId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seller> DeleteSeller([FromRoute] string sellerId)
        {
            try
            {
                var sellerList = _sellerDao.Sellers as IQueryable<Seller>;
                var seller = sellerList.First(p => p.SellerId.Equals(sellerId));

                _sellerDao.Sellers.Remove(seller);
                _sellerDao.SaveChanges();

                return Ok();
                //return new CreatedResult($"/seller/{seller.SellerId.ToLower()}", seller);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }
    }
}
