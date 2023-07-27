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
        public SubCategoriesController(ISubCategoryRepository subCategoriesRepository)
        {
            _repositoryService = subCategoriesRepository;
        }
    }
}
