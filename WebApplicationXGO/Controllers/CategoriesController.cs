using Microsoft.AspNetCore.Mvc;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;
using XGORepository.Models.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationXGO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<string[]> Get()
        {
            var dbResult = await _categoryRepository.FindAllAsync();
            return dbResult.Select(x => x.ToString()).ToArray();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string?>> Get(int id)
        {
            try
            {
                var cat = await _categoryRepository.FindByConditionAsync(x => x.Id == id);
                return cat.Any() ? Ok(cat.First()) : NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Category category)
        {
            try
            {
                var cat = await _categoryRepository.CreateAsync(category);
                return CreatedAtAction(nameof(Post), cat);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
