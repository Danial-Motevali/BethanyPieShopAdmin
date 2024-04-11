﻿using BethPieShopAdmin.Models;

namespace BethPieShopAdmin.Repository.IRepository
{
    public interface IPieRepository
    {
        Task<IEnumerable<Pie>> GetAllPiesAsync();
        Task<Pie?> GetPieByIdAsync(int pieId);
        Task<int> AddPieAsync(Pie pie);
    }
}
