using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using XGOModels;
using XGORepository.Interfaces;

namespace WebApplicationXGO.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    public class GenericController<T>(IRepositoryBase<T> repositoryService) : ControllerBase where T : BaseModel
    {
        #region Fields
        protected IRepositoryBase<T> RepositoryService => repositoryService;

        #endregion

        #region Properties

        #endregion

        #region Constructors
        //public GenericController() { }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<T[]> Get()
        {
            var dbResult = await RepositoryService.GetAllAsync();
            return dbResult.Any() ? dbResult.Select(x => x).ToArray() : Array.Empty<T>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string?>> Get(int id)
        {
            try
            {
                var cat = await RepositoryService.GetByConditionAsync(x => x.Id == id);
                return cat.Any() ? Ok(cat.First()) : NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] T baseModel)
        {
            try
            {
                var cat = await RepositoryService.CreateAsync(baseModel);
                return CreatedAtAction(nameof(Post), cat);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] T baseModel)
        {
            try
            {
                //var dbcategory = (await RepositoryService.GetByConditionAsync(x => x.Id == baseModel.Id)).FirstOrDefault();
                //if (dbcategory is null)
                //{
                //    return NoContent();
                //}
                //UpdatePropertiesExceptKey(dbcategory, baseModel);
                await RepositoryService.UpdateAsync(baseModel);
                return Accepted();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        private void UpdatePropertiesExceptKey(T obj1, T obj2)
        {
            typeof(T).GetProperties().Where(x => !x.GetCustomAttributes().Any(ca => (ca is KeyAttribute)))
                .ToList().ForEach(x =>
                {
                    x.SetValue(obj1, x.GetValue(obj2));
                });

        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var category = (await RepositoryService.GetByConditionAsync(x => x.Id == id)).FirstOrDefault();
                if (category is null)
                {
                    return NoContent();
                }
                await RepositoryService.DeleteAsync(category);
                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        protected Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> condition)
        {
            return RepositoryService.GetByConditionAsync(condition);
        }
        #endregion
    }
}
