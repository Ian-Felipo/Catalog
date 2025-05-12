using CatalogApi.Data;
using CatalogApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Category>> Get()
        {
            List<Category> categories = _catalogApiDbContext.Categories.ToList();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> Get(int id)
        {
            Category? category = _catalogApiDbContext.Categories.FirstOrDefault(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(2)]
    }   
}