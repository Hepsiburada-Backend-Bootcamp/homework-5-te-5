using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Parameters;

namespace Ecommerce.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> AddAsync(Product product);
        Task<List<Product>> GetAllAsync(ProductParameters parameters);
        Task<Product> FindByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Product product);
    }
}