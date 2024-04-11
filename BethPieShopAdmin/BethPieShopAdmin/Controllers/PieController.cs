using BethPieShopAdmin.Models;
using BethPieShopAdmin.Repository.IRepository;
using BethPieShopAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BethPieShopAdmin.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepo;
        private readonly ICategoryRepository _categoryRepo;

        public PieController(ICategoryRepository categoryRepo, IPieRepository pieRepo)
        {
            _pieRepo = pieRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var pies = await _pieRepo.GetAllPiesAsync();

            return View(pies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pie = await _pieRepo.GetPieByIdAsync(id);

            return View(pie);
        }

        public async Task<IActionResult> Add()
        {
            try
            {
                var allCategories = await _categoryRepo.GetAllCategoryAsync();
                IEnumerable<SelectListItem> selectListItem = new SelectList(allCategories, "CategoryId", "Name", null);

                PieAddViewModel pieViewModel = new()
                {
                    Categories = selectListItem
                };

                return View(pieViewModel);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Ther was an error {ex.Message}";
            }

            return View(new PieAddViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(PieAddViewModel input)
        {
            if (ModelState.IsValid)
            {
                Pie pie = new Pie()
                {
                    CategoryId = input.Pie.CategoryId,
                    ShortDescription = input.Pie.ShortDescription,
                    LongDescription = input.Pie.LongDescription,
                    Price = input.Pie.Price,
                    AllergyInformation = input.Pie.AllergyInformation,
                    ImageThumbnailUrl = input.Pie.ImageThumbnailUrl,
                    ImageUrl = input.Pie.ImageUrl,
                    InStock = input.Pie.InStock,
                    IsPieOfTheWeek = input.Pie.IsPieOfTheWeek,
                    Name = input.Pie.Name
                };

                await _pieRepo.AddPieAsync(pie);

                return RedirectToAction(nameof(Index));
            }

            var allCategories = await _categoryRepo.GetAllCategoryAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allCategories, "CategoryId", "Name", null);
            
            input.Categories = selectListItems;

            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var allCategories = await _categoryRepo.GetAllCategoryAsync();

            IEnumerable<SelectListItem> selectedListItem = new SelectList(allCategories, "CategoryId", "Name", null);

            var selectedPie = await _pieRepo.GetPieByIdAsync(id.Value);

            PieEditViewModel pieEditeViewModel = new ()
            {
                Categories = selectedListItem,
                Pie = selectedPie
            };

            return View(pieEditeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PieEditViewModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _pieRepo.UpdatePieAsync(input.Pie);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
            }catch (Exception ex)
            {
                ModelState.AddModelError("", $"Update fild {ex.Message}");
            }

            var allCategories = await _categoryRepo.GetAllCategoryAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allCategories, "CategoryId", "Name", null); 
            input.Categories = selectListItems;

            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var selectedCategory = await _pieRepo.GetPieByIdAsync(id);

            return View(selectedCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? CategoryId)
        {
            if(CategoryId == null)
            {
                ViewData["ErrorMessage"] = "Deleting the category faild, InvalidId";

                return View();
            }

            try
            {
                await _categoryRepo.DeleteCategoryAsync(CategoryId.Value);
                TempData["CategoryId"] = "Category deleted successfully!";

                return RedirectToAction(nameof(Index));

            }catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting was faild {ex.Message}";
            }

            var selectedCategory = await _categoryRepo.GetCategoryByIdAsync(CategoryId.Value);

            return View(selectedCategory);
        }
    }
}
