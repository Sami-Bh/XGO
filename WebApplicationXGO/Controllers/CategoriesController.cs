using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using XGO.Store.Application.CQRS.Category.Queries;
using XGO.Store.Models;
using XGOModels.Extras;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationXGO.Controllers
{
    [Route($"{ModulesConstants.Api}/{ModulesConstants.Categories}")]
    public class CategoriesController() : GenericController<Category, CategoryDto>()
    {
        public async override Task<ActionResult> Get(int id)
        {
            return HandleResult(await Mediator.Send(new GetCategoryDetails.Query { Id = id }));
        }

    }
}
