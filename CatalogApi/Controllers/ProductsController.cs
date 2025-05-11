using Microsoft.AspNetCore.Mvc;
using CatalogApi.Data;
using CatalogApi.Models;

namespace CatalogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogApiDbContext _catalogApiDbContext;

        public ProductsController(CatalogApiDbContext catalogApiDbContext) 
        {
            _catalogApiDbContext = catalogApiDbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            List<Product> products = _catalogApiDbContext.Products.ToList();

            if (products == null)
            {
                return NotFound();
            }            

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Get([FromRoute] int id)
        {
            Product? product = _catalogApiDbContext.Products.FirstOrDefault(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}