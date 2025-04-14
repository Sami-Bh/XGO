using BuildingBlocks.Controllers;
using Microsoft.AspNetCore.Mvc;
using XGO.Store.Application.DTOs;
using XGO.Store.Application.Filters;
using XGO.Store.Domain;
using XGO.Store.Utilities.Constants;

namespace XGO.Store.Api.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Products}")]
    public class ProductsController() : GenericController<Product, ProductDto>()
    {
        [HttpGet("GetProductsBySubCategoryId")]
        public async Task<ActionResult> GetProductsBySubCategoryId([FromQuery] ProductsFilter productsFilter)
        {
            return Ok(await Mediator.Send(new Application.CQRS.Product.Queries.GetFilteredProducts.Query
            {
                Filter = productsFilter
            }));
        }
    }
}
