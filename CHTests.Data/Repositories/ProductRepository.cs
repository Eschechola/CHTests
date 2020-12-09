using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHTests.Data.Context;
using CHTests.Data.Entities;
using CHTests.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace CHTests.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TestsContext _context;

        public ProductRepository(TestsContext context)
        {
            _context = context;
        }

        public async Task<Product> Add(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task Remove(long id)
        {
            var product = await Get(id);

            _context.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> Get(long id)
        {
            var product = await _context.Products.Where(x => x.Id == id).ToListAsync();
            return product.FirstOrDefault();
        }
    }
}
