using BethPieShopAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethPieShopAdmin.ViewModel
{
    public class PieEditViewModel
    {
        public IEnumerable<SelectListItem>? Categories { get; set; }
    
        public Pie Pie { get; set; }        
    }
}
