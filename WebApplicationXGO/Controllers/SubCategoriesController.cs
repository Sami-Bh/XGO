using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.SubCategories}")]
    [ApiController]
    public class SubCategoriesController : GenericController<SubCategory>
    {
        private readonly ICategoryRepository _categoryRepository;

        public SubCategoriesController(ISubCategoryRepository subCategoriesRepository, ICategoryRepository categoryRepository)
        {
            _repositoryService = subCategoriesRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetByCategoryIdAsync(int categoryId)
        {
            try
            {
                var aze = await _categoryRepository.GetSubCategories(categoryId);

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
