using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Models.Filters;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

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
                CategoryId = productsFilter.CategoryId,
                SubCategoryId = productsFilter.SubCategoryId,
                SearchText = productsFilter.SearchText??"",
            }));
        }
    }
}
