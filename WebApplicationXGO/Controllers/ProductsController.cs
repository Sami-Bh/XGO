using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApplicationXGO.Models.Filters;
using XGOModels;
using XGOModels.Extras;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Products}")]
    public class ProductsController() : GenericController<Product, ProductDto>()
    {
        [HttpGet("GetProductsBySubCategoryId")]
        public async Task<ActionResult> GetProductsBySubCategoryId([FromQuery] ProductsFilter productsFilter)
        {
            return Ok(await Mediator.Send(new Application.CQRS.Product.Queries.GetFilteredProducts.Query
            {
                Filter=productsFilter
            }));
        }
    }
}
