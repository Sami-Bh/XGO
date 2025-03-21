using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Products}")]
    public class ProductsController() : GenericController<Product, ProductDto>()
    {
        [HttpGet("GetProductsBySubCategoryId{subCategoryId}")]
        public async Task<ActionResult> GetProductsBySubCategoryId(int subCategoryId)
        {
            return Ok(await Mediator.Send(new Application.CQRS.Product.Queries.GetProductsBySubCategoryId.Query { SubCategoryId = subCategoryId }));
        }
    }
}
