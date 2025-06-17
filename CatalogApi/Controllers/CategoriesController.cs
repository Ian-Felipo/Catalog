using Asp.Versioning;
using CatalogApi.DTOs;
using CatalogApi.Interfaces;
using CatalogApi.Mappers;
using CatalogApi.Models;
using CatalogApi.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CatalogApi.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;
    private const string CacheCategoriesKey = "CacheCategories";

    public CategoriesController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    private string GetCacheCategoryKey(int id) => $"CacheCategory_{id}";

    private string SetCacheCategory() => 

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
        if (!_memoryCache.TryGetValue(CacheCategoriesKey, out PagedList<Category>? categories))
        {
            categories = await _unitOfWork.CategoryRepository.GetPagedListAsync(categoriesParameters);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };

            _memoryCache.Set(CacheCategoriesKey, categories, cacheOptions);
        }
        
        return Get(categories!);
    }

    [HttpGet("Filter/Name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get([FromQuery] CategoriesFilterName categoriesFilterName)
    {
        if (!_memoryCache.TryGetValue(CacheCategoriesKey, out PagedList<Category>? categories))
        {
            categories = await _unitOfWork.CategoryRepository.GetPagedListFilterNameAsync(categoriesFilterName);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };

            _memoryCache.Set(CacheCategoriesKey, categories, cacheOptions);
        }

        return Get(categories!);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryResponse>> GetById(int id, bool products = false)
    {
        string CacheCategoryKey = $"CacheCategory_{id}";

        if (!_memoryCache.TryGetValue(CacheCategoryKey, out Category? category))
        {
            category = products ? await _unitOfWork.CategoryRepository.GetWithProductsAsync(id) : await _unitOfWork.CategoryRepository.GetAsync(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };

            _memoryCache.Set(CacheCategoryKey, category, cacheOptions);
        }


        CategoryResponse categoryResponse = category!.CategoryToCategoryResponse();

        return Ok(categoryResponse);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CategoryResponse>> Post(CategoryRequest categoryRequest)
    {
        _memoryCache.Remove(CacheCategoriesKey);

        Category category = categoryRequest.CategoryRequestToCategory();

        string CacheCategoryKey = $"CacheCategory_{category.Id}";

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(10),
            Priority = CacheItemPriority.High
        };

        _memoryCache.Set(CacheCategoryKey, category, cacheOptions);

        _unitOfWork.CategoryRepository.Post(category);
        await _unitOfWork.CommitAsync();
        CategoryResponse categoryResponse = category.CategoryToCategoryResponse();

        return CreatedAtRoute("GetById", new { id = category.Id }, categoryResponse);
    }

    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryResponse>> Put(int id, CategoryRequest categoryRequest)
    {
        _memoryCache.Remove(CacheCategoriesKey);

        Category category = categoryRequest.CategoryRequestToCategory(id);

        string CacheCategoryKey = $"CacheCategory_{category.Id}";

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(10),
            Priority = CacheItemPriority.High
        };

        _memoryCache.Set(CacheCategoryKey, category, cacheOptions);

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
        string CacheCategoryKey = $"CacheCategory_{id}";

        if (!_memoryCache.TryGetValue(CacheCategoryKey, out Category? category))
        {
            category = await _unitOfWork.CategoryRepository.GetAsync(category => category.Id == id);

            if (category == null)
            {
                return NotFound();
            }
        }
        else
        {
            _memoryCache.Remove(CacheCategoryKey);
        }

        _memoryCache.Remove(CacheCategoriesKey);

        _unitOfWork.CategoryRepository.Delete(category!);
        await _unitOfWork.CommitAsync();

        CategoryResponse categoryResponse = category!.CategoryToCategoryResponse();

        return Ok(categoryResponse);
    }
}   
