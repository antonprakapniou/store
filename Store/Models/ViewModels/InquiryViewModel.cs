namespace Store.Models.ViewModels
{
    public class InquiryViewModel
    {
        public InquiryHeader? InquiryHeader { get; set; }
        public IEnumerable<InquiryDetails>? InquiryDetails { get; set; }
    }
}