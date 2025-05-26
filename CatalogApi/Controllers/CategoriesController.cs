using AutoMapper;
using CatalogApi.Data;
using CatalogApi.DTOs;
using CatalogApi.Interfaces;
using CatalogApi.Mappers;
using CatalogApi.Models;
using CatalogApi.Pagination;
using CatalogApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public ActionResult<IEnumerable<CategoryResponse>> GetCategories([FromQuery] CategoriesParameters categoriesParameters, [FromQuery] bool products = false)
        {
            var categories = products ? _unitOfWork.CategoryRepository.GetCategoriesProducts(categoriesParameters) : _unitOfWork.CategoryRepository.GetCategories(categoriesParameters);

            if (categories == null)
            {
                return NotFound();
            }

             var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasPrevious,
                categories.HasNext
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            IEnumerable<CategoryResponse> categoriesResponses = categories.Select(category => category.CategoryToCategoryResponse());

            return Ok(categoriesResponses);
        }

        [HttpGet("{id:int:min(1)}", Name="GetCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CategoryResponse> GetCategory(int id, bool products = false)
        {
            Category? category = products ? _unitOfWork.CategoryRepository.GetCategoryProducts(id) : _unitOfWork.CategoryRepository.Get(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();

            return Ok(categoryResponse);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CategoryResponse> Post(CategoryRequest categoryRequest)
        {
            Category category = categoryRequest.CategoryRequestToCategory();
            _unitOfWork.CategoryRepository.Post(category);
            _unitOfWork.Commit();
            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();
            return CreatedAtRoute("GetCategory", new { id = category.Id }, categoryResponse);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CategoryResponse> Put(int id, CategoryRequest categoryRequest)
        {
            Category category = categoryRequest.CategoryRequestToCategory(id);
            _unitOfWork.CategoryRepository.Put(category);
            _unitOfWork.Commit();
            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();
            return Ok(categoryResponse);   
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CategoryResponse> Delete(int id)
        {
            Category? category = _unitOfWork.CategoryRepository.Get(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();

            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();

            return Ok(categoryResponse);
        }
    }   
}