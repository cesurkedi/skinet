using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly  IGenericRepository<Product> _productRepo;
        private readonly  IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;

        public ProductsController(IGenericRepository<Product> productRepo, 
                                    IGenericRepository<ProductBrand> productBrandRepo,
                                    IGenericRepository<ProductType> productTypeRepo)
        {
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
        }
    
    //querysi uzun olanlarda zaman problemi yaşamamak için async komutu tercih edilir. 
    //Ve ActionResult Task içine alınır. await ve listeleme için değişiklik yapılır
    
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepo.ListAsync(spec);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
        return await _productRepo.GetByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
        return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
        return Ok(await _productTypeRepo.ListAllAsync());
        }


    }
}