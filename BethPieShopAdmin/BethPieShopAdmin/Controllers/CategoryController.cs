using BethPieShopAdmin.Models;
using BethPieShopAdmin.Repository.IRepository;
using BethPieShopAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BethPieShopAdmin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            CategoryViewModel model = new()
            {
                Categories = (await _categoryRepo.GetAllCategoryAsync()).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var selectedCategory = await _categoryRepo.GetCategoryByIdAsync(id.Value);

            return View(selectedCategory);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]//using Bind to limit the user to submite data its called over posting
        public async Task<IActionResult> Add([Bind("Name, Description, DateAdded")] Category input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryRepo.AddCategoryAsync(input);

                    return RedirectToAction(nameof(Index));
                }
            }catch (Exception ex)
            {
                ModelState.AddModelError("", $"Adding the category faild: {ex.Message}");
            }
            
            return View(input);
        }
    }
}
