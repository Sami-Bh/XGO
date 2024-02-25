using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGORepository.Models.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Categories}")]
    public class CategoriesController(ICategoryRepository categoryRepository) : GenericController<Category>(categoryRepository)
    {

    }
}
