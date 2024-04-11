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

        public async Task<int> AddCategoryAsync(Category category)
        {
            bool categoryName = await _context.categories.AnyAsync(c => c.Name == category.Name);
            
            if(categoryName)
            {
                throw new Exception("A Category with the same name already exists");
            }

            _context.categories.Add(category);

            return await _context.SaveChangesAsync();
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
