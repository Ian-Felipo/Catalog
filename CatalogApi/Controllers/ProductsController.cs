using Microsoft.AspNetCore.Mvc;
using CatalogApi.Data;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;
using CatalogApi.Filters;
using System.Data;
using System.Reflection.Metadata;
using CatalogApi.Interfaces;

namespace CatalogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(LoggingFilter))]
        public ActionResult<IEnumerable<Product>> Get()
        {
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll();

            if (products == null)
            {
                return NotFound();
            }            

            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name="GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Get(int id)
        {
            Product? product = _unitOfWork.ProductRepository.Get(product => product.Id == id);

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
            _unitOfWork.ProductRepository.Post(product);
            _unitOfWork.Commit();
            return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ProductRepository.Put(product);
            _unitOfWork.Commit();
          
            return Ok(product);
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Product> Delete(int id)
        {
            Product? product = _unitOfWork.ProductRepository.Get(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();

            return Ok(product);
        }
    }
}