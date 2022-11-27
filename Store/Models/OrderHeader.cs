using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string? CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public IdentityUser? CreatedBy { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; }

        [Required]
        public double FinalOrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}