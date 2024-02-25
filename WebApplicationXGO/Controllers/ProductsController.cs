using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api} /{ModulesConstants.Products}")]
    public class ProductsController(IProductRepository productRepository) : GenericController<Product>(productRepository)
    {
    }
}
