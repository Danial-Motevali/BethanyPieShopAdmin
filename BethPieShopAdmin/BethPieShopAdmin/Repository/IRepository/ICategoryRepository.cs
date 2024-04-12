using BethPieShopAdmin.Models;

namespace BethPieShopAdmin.Repository.IRepository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<int> AddCategoryAsync(Category category);
        Task<int> UpdateCategoryAsync (Category category);
        Task<int> DeleteCategoryAsync (int id);
        Task<int> UpdateCategoryNameAsync(List<Category> categories);
    }
}
