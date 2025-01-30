using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGORepository.Models.Repositories;
using XGOUtilities.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Categories}")]
    public class CategoriesController(ICategoryRepository categoryRepository, ICategoryUnitOfWork categoryUnitOfWork) : GenericController<Category>(categoryRepository)
    {
        [HttpGet($"{ControllerActions.GetCategoriesIncludeSubCategories}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesIncludeSubCategoriesAsync()
        {
            try
            {
                return Ok(await categoryUnitOfWork.GetCategoriesWithSubCategoriesAsync());
            }
            catch 
            {
                return StatusCode(500);
            }
        }
    }
}
