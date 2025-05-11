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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Product> Create([FromBody] Product productCreate)
        {
            _catalogApiDbContext.Products.Add(productCreate);
            _catalogApiDbContext.SaveChanges();
            return CreatedAtAction(nameof(Create), new { id = productCreate.Id }, productCreate);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Update([FromRoute] int id, [FromBody] Product productUpdate)
        {
            Product? product = _catalogApiDbContext.Products.FirstOrDefault(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = productUpdate.Name;
            product.Description = productUpdate.Description;
            product.Price = productUpdate.Price;
            product.ImageUrl = productUpdate.ImageUrl;
            product.Stock = productUpdate.Stock;
            product.RegistrationDate = productUpdate.RegistrationDate;
            product.CategoryId = productUpdate.CategoryId;
            product.Category = productUpdate.Category;

            _catalogApiDbContext.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Delete([FromRoute] int id)
        {
            Product? product = _catalogApiDbContext.Products.FirstOrDefault(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _catalogApiDbContext.Products.Remove(product);
            _catalogApiDbContext.SaveChanges();

            return Ok(product);
        }
    }
}