using System.ComponentModel.DataAnnotations;

namespace BethPieShopAdmin.Models
{
    public class Pie
    {
        public int PieId { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Short Description")]
        public string? ShortDescription { get; set; }

        [StringLength(1000)]
        [Display(Name = "Long Description")]
        public string? LongDescription { get; set; }

        [StringLength(1000)]
        [Display(Name = "Allergy information")]
        public string? AllergyInformation { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageThumbnailUrl { get; set; }

        public bool IsPieOfTheWeek { get; set; }

        public bool InStock { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<Ingredient>? Ingredients { get; set; }

    }
}
