using BethPieShopAdmin.Data;
using BethPieShopAdmin.Models;
using BethPieShopAdmin.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BethPieShopAdmin.Repository
{
    public class PieRepository : IPieRepository
    {
        private readonly ApplicationDbContext _bethanysPieShopDbContext;

        public PieRepository(ApplicationDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        public async Task<IEnumerable<Pie>> GetAllPiesAsync()
        {
            return await _bethanysPieShopDbContext.pies.OrderBy(c => c.PieId).AsNoTracking().ToListAsync();
        }

        public async Task<Pie?> GetPieByIdAsync(int pieId)
        {
            return await _bethanysPieShopDbContext.pies.Include(p => p.Ingredients).Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.PieId == pieId);
        }

        public async Task<int> AddPieAsync(Pie pie)
        {
            _bethanysPieShopDbContext.pies.Add(pie);//could be done using async too
            return await _bethanysPieShopDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdatePieAsync(Pie pie)
        {
            var pieToUpdate = await _bethanysPieShopDbContext.pies.FirstOrDefaultAsync(c => c.PieId == pie.PieId);

            if(pieToUpdate != null)
            {
                pieToUpdate.CategoryId = pie.CategoryId;
                pieToUpdate.ShortDescription = pie.ShortDescription;
                pieToUpdate.LongDescription = pie.LongDescription;
                pieToUpdate.Price = pie.Price;
                pieToUpdate.AllergyInformation = pie.AllergyInformation;
                pieToUpdate.ImageThumbnailUrl = pie.ImageThumbnailUrl;
                pieToUpdate.ImageUrl = pie.ImageUrl;
                pieToUpdate.InStock = pie.InStock;
                pieToUpdate.IsPieOfTheWeek = pie.IsPieOfTheWeek;
                pieToUpdate.Name = pie.Name;

                _bethanysPieShopDbContext.Update(pieToUpdate);
                return await _bethanysPieShopDbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"cant find the Pie");
            }
        }

        public Task<int> DeletePieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetAllPiesCountAsync()
        {
            var count = await _bethanysPieShopDbContext.pies.CountAsync(); 
            return count;
        }

        public async Task<IEnumerable<Pie>> GetPiesPagedAsync(int? pageNumber, int pageSize)
        {
            IQueryable<Pie> pies = from p in _bethanysPieShopDbContext.pies
                                   select p;

            pageNumber ??= 1;

            pies = pies.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            return await pies.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Pie>> GetPiesSortedAndPagedAsync(string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable <Pie> pies = from p in _bethanysPieShopDbContext.pies
                                 select p;

            switch(sortBy)
            {
                case "name_desc":
                    pies = pies.OrderByDescending(p => p.Name);
                    break;
                case "name":
                    pies = pies.OrderBy(p => p.Name);
                    break;
                case "id_desc":
                    pies = pies.OrderByDescending(p => p.PieId);
                    break;
                case "id":
                    pies = pies.OrderBy(p => p.PieId);
                    break;
                case "price_desc":
                    pies = pies.OrderByDescending(p => p.Price);
                    break;
                case "price":
                    pies = pies.OrderBy(p => p.Price);
                    break;
                default:
                    pies = pies.OrderBy(p => p.PieId);
                    break;
            }

            pageNumber ??= 1;

            pies = pies.Skip((pageNumber.Value -1) * pageSize).Take(pageSize);

            return await pies.AsNoTracking().ToListAsync();
        }
    }
}
