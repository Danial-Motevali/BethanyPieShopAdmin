﻿using BethPieShopAdmin.Models;
using BethPieShopAdmin.Repository.IRepository;
using BethPieShopAdmin.Utilities;
using BethPieShopAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Edit(Pie input)
        {
            Pie pieToUpdate = await _pieRepo.GetPieByIdAsync(input.PieId);

            try
            {
                if(pieToUpdate == null)
                {
                    ModelState.AddModelError(string.Empty, "cont find the pie");
                }

                if (ModelState.IsValid)
                {
                    await _pieRepo.UpdatePieAsync(input);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionsPie = ex.Entries.Single();
                var entityValues = (Pie)exceptionsPie.Entity;
                var databsePie = exceptionsPie.GetDatabaseValues();

                if (databsePie == null)
                {
                    ModelState.AddModelError(string.Empty, "The pie was deleted by some one else");
                }
                else
                {
                    var databaseValues = (Pie)databsePie.ToObject();

                    if(databaseValues.Name != entityValues.Name)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie name {databaseValues.Name}");
                    }
                    
                    if(databaseValues.Price != entityValues.Price)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie price {databaseValues.Price}");
                    }

                    if(databaseValues.ShortDescription != entityValues.ShortDescription)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie shortdescription {databaseValues.ShortDescription}");
                    }

                    if (databaseValues.LongDescription != entityValues.LongDescription)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie longdescriptiono {databaseValues.LongDescription}");
                    }

                    if (databaseValues.AllergyInformation != entityValues.AllergyInformation)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie allergyinformation {databaseValues.AllergyInformation}");
                    }

                    if (databaseValues.Ingredients != entityValues.Ingredients)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie ingredients {databaseValues.Ingredients}");
                    }

                    if (databaseValues.CategoryId != entityValues.CategoryId)
                    {
                        ModelState.AddModelError(string.Empty, $"current pie categoryId {databaseValues.CategoryId}");
                    }

                    ModelState.AddModelError(string.Empty, "The Pie was modified already by another user");
                    pieToUpdate.RowVersion = databaseValues.RowVersion;

                    ModelState.Remove("Pie.RowVersion");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Update faild pleas try again leater {ex.Message}");
            }

            var allCategories = await _categoryRepo.GetAllCategoryAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allCategories, "CategoryId", "Name", null);

            PieEditViewModel pieEditViewModel = new()
            {
                Categories = selectListItems,
                Pie = pieToUpdate
            };

            return View(pieEditViewModel);
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

        private int pageSize = 5;

        public async Task<IActionResult> IndexPaging(int? pageNumber)
        {
            var pies = await _pieRepo.GetPiesPagedAsync(pageNumber, pageSize);
            pageNumber ??= 1;

            var count = await _pieRepo.GetAllPiesCountAsync();

            return View(new PagedList<Pie>(pies.ToList(), count, pageNumber.Value, pageSize));
        }

        public async Task<IActionResult> IndexPagingSorting(string sortBy, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortBy;
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortBy) || sortBy == "id_desc" ? "id" : "id_desc";
            ViewData["NameSortParam"] = sortBy == "name" ? "name_desc" : "name";
            ViewData["PriceSortParam"] = sortBy == "price" ? "price_desc" : "price";

            pageNumber ??= 1;

            var pies = await _pieRepo.GetPiesSortedAndPagedAsync(sortBy, pageNumber, pageSize);

            var count = await _pieRepo.GetAllPiesCountAsync();

            return View(new PagedList<Pie>(pies.ToList(), count, pageNumber.Value, pageSize));
        }
    
        public async Task<IActionResult> Search(string? searchQuery, int? searchCategory)
        {
            var allCategories = await _categoryRepo.GetAllCategoryAsync();

            IEnumerable<SelectListItem> selectListItems = new SelectList(allCategories, "CategoryId", "Name", null);
        
            if(searchQuery != null)
            {
                var pies = await _pieRepo.SearchPies(searchQuery, searchCategory);

                return View(new PieSearchViewModel()
                {
                    Pies = pies,
                    SearchCategory = searchCategory,
                    Categories = selectListItems,
                    SearchQuery = searchQuery
                });
            }

             return View(new PieSearchViewModel()
             {
                 Pies = new List<Pie>(),
                 SearchCategory = searchCategory,
                 Categories = selectListItems,
                 SearchQuery = searchQuery
             });
        }
    }
}
