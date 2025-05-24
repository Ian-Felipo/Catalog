using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using CatalogApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> GetCategories(bool products = false)
        {
            IEnumerable<Category> categories = products ? _unitOfWork.CategoryRepository.GetCategoriesProducts() : _unitOfWork.CategoryRepository.GetAll();

            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id:int:min(1)}", Name="GetCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> GetCategory(int id, bool products = false)
        {
            Category? category = products ? _unitOfWork.CategoryRepository.GetCategoryProducts(id) : _unitOfWork.CategoryRepository.Get(category => category.Id == id);

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
            _unitOfWork.CategoryRepository.Post(category);
            _unitOfWork.Commit();
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

            _unitOfWork.CategoryRepository.Put(category);
            _unitOfWork.Commit();

            return Ok(category);   
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Category> Delete(int id)
        {
            Category? category = _unitOfWork.CategoryRepository.Get(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();

            return Ok(category);
        }
    }   
}