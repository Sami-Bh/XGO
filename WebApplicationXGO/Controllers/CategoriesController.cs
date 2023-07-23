using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGORepository.Models.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationXGO.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    //[ApiController]
    public class CategoriesController : GenericController<Category>
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _repositoryService = categoryRepository;
        }

        //// GET: api/<CategoriesController>
        //[HttpGet]
        //public async Task<string[]> Get()
        //{
        //    var dbResult = await _categoryRepository.GetAllAsync();
        //    return dbResult.Any() ? dbResult.Select(x => x.ToString()).ToArray() : Array.Empty<string>();
        //}

        // GET api/<CategoriesController>/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<string?>> Get(int id)
        //{
        //    try
        //    {
        //        var cat = await _categoryRepository.GetByConditionAsync(x => x.Id == id);
        //        return cat.Any() ? Ok(cat.First()) : NotFound();
        //    }
        //    catch (Exception e)
        //    {
        //        return NotFound();
        //    }
        //}

        // POST api/<CategoriesController>
        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] Category category)
        //{
        //    try
        //    {
        //        var cat = await _categoryRepository.CreateAsync(category);
        //        return CreatedAtAction(nameof(Post), cat);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500);
        //    }
        //}

        // PUT api/<CategoriesController>/5
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Category category)
        //{
        //    try
        //    {
        //        var dbcategory = (await _categoryRepository.GetByConditionAsync(x => x.Id == category.Id)).FirstOrDefault();
        //        if (dbcategory is null)
        //        {
        //            return NoContent();
        //        }
        //        dbcategory = category;
        //        await _categoryRepository.UpdateAsync(dbcategory);
        //        return Accepted();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //}

        // DELETE api/<CategoriesController>/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var category = (await _categoryRepository.GetByConditionAsync(x => x.Id == id)).FirstOrDefault();
        //        if (category is null)
        //        {
        //            return NoContent();
        //        }
        //        await _categoryRepository.DeleteAsync(category);
        //        return Accepted();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //}
    }
}
