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
    }
}
