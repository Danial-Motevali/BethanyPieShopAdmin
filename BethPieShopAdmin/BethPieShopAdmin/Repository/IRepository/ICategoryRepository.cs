using BethPieShopAdmin.Models;

namespace BethPieShopAdmin.Repository.IRepository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();

        Task<IEnumerable<Category>> GetAllCategoryAsync();

        Task<Category?> GetCategoryByIdAsync(int id);
    }
}
