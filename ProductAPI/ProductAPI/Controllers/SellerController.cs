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
        private readonly ProductContext _sellerDao;

        public SellerController(ProductContext context)
        {
            _sellerDao = context;

            if (_sellerDao.Products.Any()) return;

            ProductSeed.InitData(context);
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

                return new CreatedResult($"/sellers/{seller.SellerId.ToLower()}", seller);
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

        //getting error need to test
        [HttpPatch]
        [Route("{sellerId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //copy over PatchProduct information and add seller info so you can change seller info
        public ActionResult<Seller> UpdateSeller([FromRoute] string sellerId, [FromBody] SellerPatch newSeller, [FromRoute] string inventoryId )
        {
            try
            {
                var sellerList = _sellerDao.Sellers as IQueryable<Seller>;
                var seller = sellerList.First(p => p.SellerId.Equals(sellerId));
                var inventorystuff = seller.ProductsSold.First(p => p.InventoryId.Equals(inventoryId));
                //trying to get stock changing, may need new model, trying to get numbersold to reduce numberheld
                seller.SellerName = newSeller.SellerName ?? seller.SellerName;
                seller.ProductsSold = newSeller.ProductsSold ?? seller.ProductsSold;
                inventorystuff.NumberSold += 1;
                inventorystuff.NumberHeld -= 1;
                //Don't need stock since we have inventoryitem class
                //seller.Stock = newSeller.Stock ?? seller.Stock;                    
                
               
                _sellerDao.Sellers.Update(seller);
                _sellerDao.SaveChanges();

                return new CreatedResult($"/sellers/{seller.SellerId.ToLower()}", seller);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }

        [HttpDelete]
        [Route("{sellerId}")]
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
                //return new CreatedResult($"/sellers/{seller.SellerId.ToLower()}", seller);
            }
            catch (Exception e)
            {
                // Typically an error log is produced here
                return ValidationProblem(e.Message);
            }
        }
    }
}
