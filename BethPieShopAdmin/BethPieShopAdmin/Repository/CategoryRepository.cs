using BethPieShopAdmin.Data;
using BethPieShopAdmin.Models;
using BethPieShopAdmin.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BethPieShopAdmin.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.categories.OrderBy(c => c.CategoryId).AsNoTracking();
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _context.categories.AsNoTracking().OrderBy(c => c.CategoryId).ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.categories.Include(p => p.Pies).FirstOrDefaultAsync(c => c.CategoryId == id);
        }
    }
}
