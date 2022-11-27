using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class InquiryHeader
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        public DateTime InquiryDate { get; set; }
        public required string? Email { get; set; }
    }
}