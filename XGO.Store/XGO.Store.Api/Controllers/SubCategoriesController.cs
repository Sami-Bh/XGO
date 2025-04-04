using Microsoft.AspNetCore.Mvc;
using XGO.Store.Application.CQRS.Subcategories.Queries;
using XGO.Store.Application.DTOs;
using XGO.Store.Domain;
using XGO.Store.Utilities.Constants;

namespace XGO.Store.Api.Controllers
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
