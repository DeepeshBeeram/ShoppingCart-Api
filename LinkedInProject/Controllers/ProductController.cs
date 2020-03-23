using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkedInProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkedInProject.Helpers;

namespace LinkedInProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;

            _context.Database.EnsureDeleted();

            _context.Database.EnsureCreated();
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters )
        {
            IQueryable<Product> products = _context.Products;

            if(queryParameters.MaxPrice != null && queryParameters.MinPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value
                && p.Price <= queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            products = products.
                Skip(queryParameters.Size * (queryParameters.Page - 1)).
                Take(queryParameters.Size);

            var allProducts = await products.ToArrayAsync();
            return Ok(allProducts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct( int id)
        {
            var product = await _context.Products.FindAsync(id);
            return Ok(product);
        }
    }
}