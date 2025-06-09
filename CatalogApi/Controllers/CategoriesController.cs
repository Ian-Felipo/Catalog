using CatalogApi.DTOs;
using CatalogApi.Interfaces;
using CatalogApi.Mappers;
using CatalogApi.Models;
using CatalogApi.Pagination;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

        private ActionResult<IEnumerable<CategoryResponse>> Get(PagedList<Category> categories)
        {
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            PagedList<Category> categories = await _unitOfWork.CategoryRepository.GetPagedListAsync(categoriesParameters);
            return Get(categories);
        }

        [HttpGet("Filter/Name")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get([FromQuery] CategoriesFilterName categoriesFilterName)
        {
            PagedList<Category> categories = await _unitOfWork.CategoryRepository.GetPagedListFilterNameAsync(categoriesFilterName);
            return Get(categories);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryResponse>> GetById(int id, bool products = false)
        {
            Category? category;

            if (products)
            {
                category = await _unitOfWork.CategoryRepository.GetWithProductsAsync(id); 
            }
            else
            {
                category = await _unitOfWork.CategoryRepository.GetAsync(category => category.Id == id);
            }

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
        public async Task<ActionResult<CategoryResponse>> Post(CategoryRequest categoryRequest)
        {
            Category category = categoryRequest.CategoryRequestToCategory();
            _unitOfWork.CategoryRepository.Post(category);
            await _unitOfWork.CommitAsync();
            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();
            return CreatedAtRoute("GetCategory", new { id = category.Id }, categoryResponse);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryResponse>> Put(int id, CategoryRequest categoryRequest)
        {
            Category category = categoryRequest.CategoryRequestToCategory(id);
            _unitOfWork.CategoryRepository.Put(category);
            await _unitOfWork.CommitAsync();
            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();
            return Ok(categoryResponse);   
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryResponse>> Delete(int id)
        {
            Category? category = await _unitOfWork.CategoryRepository.GetAsync(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();

            CategoryResponse categoryResponse = category.CategoryToCategoryResponse();

            return Ok(categoryResponse);
        }
    }   
}