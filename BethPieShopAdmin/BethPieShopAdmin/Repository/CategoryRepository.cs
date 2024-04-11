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

        public async Task<int> DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _context.categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            var piesInCategory = _context.pies.Any(p => p.CategoryId == id);

            if(piesInCategory)
            {
                throw new Exception("Pies Exist in this category. Delete Them");
            }

            if(categoryToDelete != null)
            {
                _context.categories.Remove(categoryToDelete);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("cant found Category");
            }
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

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            bool categoryWithSameName = await _context.categories.AnyAsync(p => p.Name == category.Name && p.CategoryId != category.CategoryId);
            if(categoryWithSameName)
            {
                throw new Exception("A category with the same name already exists");
            }
          
            var categoryToUpdate = await _context.categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if(categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Description = category.Description;

                _context.categories.Update(categoryToUpdate);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"This category cant be found");
            }
        }
    }
}
