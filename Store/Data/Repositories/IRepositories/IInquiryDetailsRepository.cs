using Store.Models;

namespace Store.Data.Repositories.IRepositories
{
    public interface IInquiryDetailsRepository:IRepository<InquiryDetails>
    {
        public void Update(InquiryDetails inquiryDetails);
    }
}