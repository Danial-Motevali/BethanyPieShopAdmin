using System.ComponentModel.DataAnnotations;

namespace BethPieShopAdmin.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        [StringLength(50, ErrorMessage = "The name should be no longer that 50")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "The amount should be no longer that 100 characters")]
        [Display(Name = "Amount")]
        public string Amount { get; set; } = string.Empty;
    }
}
