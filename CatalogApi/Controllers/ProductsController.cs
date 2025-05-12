using Microsoft.AspNetCore.Mvc;
using CatalogApi.Data;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

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
            List<Product> products = _catalogApiDbContext.Products.AsNoTracking().ToList();

            if (products == null)
            {
                return NotFound();
            }            

            return Ok(products);
        }

        [HttpGet("{id:int}", Name="GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Get(int id)
        {
            Product? product = _catalogApiDbContext.Products.AsNoTracking().FirstOrDefault(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Product> Post(Product product)
        {
            _catalogApiDbContext.Products.Add(product);
            _catalogApiDbContext.SaveChanges();
            return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                _catalogApiDbContext.Entry(product).State = EntityState.Modified; // *******************************************
                _catalogApiDbContext.SaveChanges();
            }
            catch 
            {
                return BadRequest($"Nao existe uma Categoria com o Id {id} no banco de dados");
            }

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Delete(int id)
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