using CatalogApi.Data;
using CatalogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CatalogApiDbContext _catalogApiDbContext;
        private readonly ILogger _logger;

        public CategoriesController(CatalogApiDbContext catalogApiDbContext, ILogger logger)
        {
            _catalogApiDbContext = catalogApiDbContext;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            _logger.LogInformation("==================== GET /Categories ====================");

            List<Category> categories = _catalogApiDbContext.Categories.AsNoTracking().ToList();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id:int:min(1)}", Name="GetCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> GetCategory(int id)
        {
            _logger.LogInformation($"==================== GET /Categories/{id} ====================");

            Category? category = _catalogApiDbContext.Categories.AsNoTracking().FirstOrDefault(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("Products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            _logger.LogInformation("==================== GET /Categories/Products ====================");

            List<Category> categories = _catalogApiDbContext.Categories.AsNoTracking().Include(category => category.Products).ToList();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id:int:min(1)}/Products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> GetCategoryProducts(int id)
        {
            _logger.LogInformation($"==================== GET /Categories/Products/{id} ====================");

            Category? category = _catalogApiDbContext.Categories.AsNoTracking().Include(category => category.Products).FirstOrDefault(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        } 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> Post(Category category)
        {
            _catalogApiDbContext.Categories.Add(category);
            _catalogApiDbContext.SaveChanges();
            return CreatedAtRoute("GetCategory", new { Id = category.Id }, category);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> Put(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            
            try 
            {
                _catalogApiDbContext.Entry(category).State = EntityState.Modified;
                _catalogApiDbContext.SaveChanges();
            }
            catch 
            {
                return BadRequest($"Nao existe uma Categoria com o Id {id} no banco de dados");
            }

            return Ok(category);   
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> Delete(int id)
        {
            Category? category = _catalogApiDbContext.Categories.FirstOrDefault(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _catalogApiDbContext.Categories.Remove(category);
            _catalogApiDbContext.SaveChanges();

            return Ok(category);
        }
    }   
}