using BethPieShopAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethPieShopAdmin.ViewModel
{
    public class PieAddViewModel
    {
        public IEnumerable<SelectListItem>? Categories { get; set; } = default!;
        
        public Pie? Pie { get; set; }
    }
}
