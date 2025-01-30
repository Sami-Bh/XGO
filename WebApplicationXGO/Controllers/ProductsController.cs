using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Products}")]
    public class ProductsController(IProductRepository productRepository, IProductUnitOfWork productUnitOfWork) : GenericController<Product>(productRepository)
    {
        [HttpGet]
        public override async Task<ActionResult<Product[]>> Get()
        {
            return Ok(await productUnitOfWork.GetProductsIncludeSubCategoryAsync());
        }
    }
}
