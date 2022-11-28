using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "The price value is invalid")]
        public double Price { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        [Range(1, 10000,ErrorMessage ="Count must be greater than 0")]
        public int Temp { get; set; }

        public Product()
        {
            Temp=1;
        }
    }
}