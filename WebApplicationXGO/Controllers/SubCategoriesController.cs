using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGOModels.Extras;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGOUtilities.Extensions;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.SubCategories}")]
    [ApiController]
    public class SubCategoriesController(ISubCategoryRepository subCategoriesRepository, ISubCategoryUnitOfWork subCategoryUnitOfWork) : GenericController<SubCategory>(subCategoriesRepository)
    {
        [HttpGet($"{ActionConstants.GetByCategoryId}")]

        public async Task<ActionResult<SubCategory[]>> GetByCategoryIdAsync(int id)
        {
            try
            {
                return Ok(await subCategoryUnitOfWork.GetSubCategoriesByCategoryIdAsync(id));

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost($"{ActionConstants.Create}")]
        public async Task<ActionResult<SubCategory>> Create(int categoryId, [FromBody] SubCategory subCategory)
        {
            try
            {
                if (categoryId <= 0 || subCategory is null)
                {
                    return BadRequest();
                }
                return CreatedAtAction(nameof(Create), await subCategoriesRepository.CreateAsync(categoryId, subCategory));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
