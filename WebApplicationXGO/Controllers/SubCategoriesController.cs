using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.SubCategories}")]
    [ApiController]
    public class SubCategoriesController(ISubCategoryRepository subCategoriesRepository, ISubCategoryUnitOfWork subCategoryUnitOfWork) : GenericController<SubCategory>(subCategoriesRepository)
    {

        //public SubCategoriesController(ISubCategoryRepository subCategoriesRepository)
        //{
        //    _repositoryService = subCategoriesRepository;
        //}

        [HttpGet("GetByCategoryId/{categoryId}")]
        
        public async Task<ActionResult<SubCategory[]>> GetByCategoryIdAsync(int categoryId)
        {
            try
            {
                return Ok(await subCategoryUnitOfWork.GetSubCategoriesByCategoryIdAsync(categoryId));

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
