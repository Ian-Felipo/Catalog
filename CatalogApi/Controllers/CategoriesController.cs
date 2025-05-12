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

        public CategoriesController(CatalogApiDbContext catalogApiDbContext)
        {
            _catalogApiDbContext = catalogApiDbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            List<Category> categories = _catalogApiDbContext.Categories.AsNoTracking().ToList();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id:int}", Name="GetCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> GetCategory(int id)
        {
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
            List<Category> categories = _catalogApiDbContext.Categories.AsNoTracking().Include(category => category.Products).ToList();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id:int}/Products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> GetCategoryProducts(int id)
        {
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

        [HttpPut]
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

        [HttpDelete]
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