using Application.CQRS.Subcategories.Queries;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGOModels.Extras;

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.SubCategories}")]
    [ApiController]
    public class SubCategoriesController() : GenericController<SubCategory, SubCategoryDto>
    {
        public async override Task<ActionResult> Get(int id)
        {
            return HandleResult(await Mediator.Send(new GetSubcategoryDetails.Query { Id = id }));
        }

        public async override Task<ActionResult> Get()
        {
            return await Task.Factory.StartNew(NoContent);
        }

        [HttpGet("GetSubcategoriesListByCategoryId/{id}")]
        public async Task<ActionResult> GetSubcategoriesListByCategoryId(int id)
        {
            return Ok(await Mediator.Send(new GetSubcategoriesListByCategoryId.Query { CategoryId = id }));
        }
    }
}
