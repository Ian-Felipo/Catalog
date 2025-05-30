using Microsoft.AspNetCore.Mvc;
using CatalogApi.Models;
using CatalogApi.Filters;
using CatalogApi.Interfaces;
using AutoMapper;
using CatalogApi.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using CatalogApi.Pagination;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CatalogApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private ActionResult<IEnumerable<ProductResponse>> Get(PagedList<Product> products)
        {
            if (products == null)
            {
                return NotFound();
            }

            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasPrevious,
                products.HasNext
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            IEnumerable<ProductResponse> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);

            return Ok(productResponses);
        }

        [Authorize] //////////////////////////////////////////////////////
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            PagedList<Product> products = await _unitOfWork.ProductRepository.GetProductsPagedListAsync(productsParameters);
            return Get(products);
        }

        [HttpGet("Filter/Price")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Get([FromQuery] ProductsFilterPrice productsFilterPrice)
        {
            PagedList<Product> products = await _unitOfWork.ProductRepository.GetProductsPagedListFilterPriceAsync(productsFilterPrice);
            return Get(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponse>> Get(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.GetAsync(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            return Ok(productResponse);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductResponse>> Post(ProductRequest productRequest)
        {
            Product product = _mapper.Map<Product>(productRequest);
            _unitOfWork.ProductRepository.Post(product);
            await _unitOfWork.CommitAsync();
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
            return CreatedAtRoute("GetProduct", new { Id = productResponse.Id }, productResponse);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponse>> Put(int id, ProductRequest productRequest)
        {
            Product product = _mapper.Map<Product>(productRequest);
            product.Id = id;
            _unitOfWork.ProductRepository.Put(product);
            await _unitOfWork.CommitAsync();
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(productResponse);
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponse>> Delete(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.GetAsync(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            return Ok(productResponse);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResponse>> Patch(int id, JsonPatchDocument<ProductRequest> patchDoc)
        {
            Product? product = await _unitOfWork.ProductRepository.GetAsync(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ProductRequest productRequest = _mapper.Map<ProductRequest>(product);

            patchDoc.ApplyTo(productRequest, ModelState);

            product = _mapper.Map<Product>(productRequest);
            product.Id = id;

            _unitOfWork.ProductRepository.Put(product);
            await _unitOfWork.CommitAsync();

            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            return Ok(productResponse);
        }
    }
}