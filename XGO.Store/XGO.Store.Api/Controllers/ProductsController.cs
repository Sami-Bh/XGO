using BuildingBlocks.Controllers;
using Microsoft.AspNetCore.Mvc;
using XGO.Store.Application.CQRS.Product.Queries;
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
            return Ok(await Mediator.Send(new GetFilteredProducts.Query
            {
                Filter = productsFilter
            }));
        }

        [HttpGet("GetProductNamesBySubCategory/{subCategoryId}")]
        public async Task<ActionResult<List<GetProductNamesBySubCategory.ProductInfo>>> GetProductNamesBySubCategory(int subCategoryId)
        {
            return Ok(await Mediator.Send(new GetProductNamesBySubCategory.Query
            {
                SubCategoryId = subCategoryId
            }));
        }
    }
}
