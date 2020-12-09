using CHTests.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CHTests.Data.Interface
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
        Task Remove(long id);
        Task<List<Product>> Get();
        Task<Product> Get(long id);
    }
}
