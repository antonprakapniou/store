using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class InquiryDetails
    {
        [Key]
        public int Id { get; set; }
        public required int InquiryHeaderId { get; set; }

        [ForeignKey("InquiryHeaderId")]
        public InquiryHeader? InquiryHeader { get; set; }
        public required int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}