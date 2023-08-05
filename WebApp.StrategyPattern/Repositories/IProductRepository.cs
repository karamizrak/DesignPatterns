using WebApp.StrategyPattern.Models;

namespace WebApp.StrategyPattern.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(string id);
        Task<List<Product>> GetAllByUserId (string userId);
        Task<Product> Save(Product product);
        Task Delete(Product product);
        Task Update(Product product);
    }
}
