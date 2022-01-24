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

        //public SellerController()
        //{
        //_sellerDao = sellerdao;

        //if (_context.Products.Any()) return;


        //}
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seller> PostSeller([FromBody] Seller seller)
        {

            try
            {
                _sellerDao.Sellers.Add(seller);
                _sellerDao.SaveChanges();

                return new CreatedResult($"/seller/{seller.Id.ToLower()}", seller);
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

            return Ok(result.OrderBy(p => p.Id).Take(15));
        }


        //[HttpPatch]
        //[Route("{Id}")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]

        //[HttpDelete]
        //[Route("{productNumber}")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<Product> DeleteProduct([FromRoute] string productNumber)
        //{
        //    try
        //    {
        //        var productList = _context.Products as IQueryable<Product>;
        //        var product = productList.First(p => p.ProductNumber.Equals(productNumber));

        //        _context.Products.Remove(product);
        //        _context.SaveChanges();

        //        return new CreatedResult($"/products/{product.ProductNumber.ToLower()}", product);
        //    }
        //    catch (Exception e)
        //    {
        //        // Typically an error log is produced here
        //        return ValidationProblem(e.Message);
        //    }
        //}
    }
}
