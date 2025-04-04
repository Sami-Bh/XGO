using BuildingBlocks.Controllers;
using BuildingBlocks.Core;
using BuildingBlocks.CQRS.Generic.Commands;
using BuildingBlocks.CQRS.Generic.Queries;
using BuildingBlocks.DTOs;
using BuildingBlocks.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XGO.Store.Api.Controllers
{

    [ApiController]
    public class GenericController<dbT, dtoT>() : AuthorizedControllerBase where dbT : BaseModel where dtoT : BaseDto
    {
        #region Fields
        private ILogger? _logger;
        protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger>() ?? throw new InvalidOperationException(nameof(Logger));

        private IMediator? _mediator;
        public IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? throw new InvalidOperationException(nameof(Mediator));
        #endregion

        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsValid && result.Value is not null)
            {
                return Ok(result.Value);
            }

            if (result.ErrorCode == 404) return NotFound();

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("test")]
        public async Task<ActionResult<string>> Test()
        {
            return await Mediator.Send(new TestQuery.Query());
        }

        [HttpGet]
        public virtual async Task<ActionResult> Get()
        {
            var result = await Mediator.Send(new GetIList<dbT, dtoT>.Query());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> Get(int id)
        {

            var result = await Mediator.Send(new GetItem<dbT, dtoT>.Query() { Id = id });
            return HandleResult(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post([FromBody] dtoT dto)
        {
            return HandleResult(await Mediator.Send(new CreateItem<dbT, dtoT>.Command() { Dto = dto }));
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {

            var result = await Mediator.Send(new DeleteItem<dbT, dtoT>.Command() { Id = id });
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] dtoT dto)
        {
            var result = await Mediator.Send(new EditItem<dbT, dtoT>.Command() { Dto = dto });
            return HandleResult(result);
        }

        #region Old Code
        //[HttpPost]
        //public async Task<ActionResult<int>> Post([FromBody] T baseModel)
        //{
        //    try
        //    {
        //        var cat = await RepositoryService.CreateAsync(baseModel);
        //        return Ok(cat.Id);
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogError(e, nameof(Post));

        //        return StatusCode(500);
        //    }
        //}

        //[HttpPut]
        //public async Task<ActionResult> Put([FromBody] T baseModel)
        //{
        //    try
        //    {
        //        //var dbcategory = (await RepositoryService.GetByConditionAsync(x => x.Id == baseModel.Id)).FirstOrDefault();
        //        //if (dbcategory is null)
        //        //{
        //        //    return NoContent();
        //        //}
        //        //UpdatePropertiesExceptKey(dbcategory, baseModel);
        //        await RepositoryService.UpdateAsync(baseModel);
        //        return Accepted();
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500);
        //    }
        //}

        //private void UpdatePropertiesExceptKey(T obj1, T obj2)
        //{
        //    typeof(T).GetProperties().Where(x => !x.GetCustomAttributes().Any(ca => (ca is KeyAttribute)))
        //        .ToList().ForEach(x =>
        //        {
        //            x.SetValue(obj1, x.GetValue(obj2));
        //        });

        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var category = (await RepositoryService.GetByConditionAsync(x => x.Id == id)).FirstOrDefault();
        //        if (category is null)
        //        {
        //            return NotFound();
        //        }
        //        await RepositoryService.DeleteAsync(category);
        //        return Accepted();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //}

        //protected Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> condition)
        //{
        //    return RepositoryService.GetByConditionAsync(condition);
        //}
        #endregion
        #endregion
    }
}
