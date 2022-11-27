using Store.Models;

namespace Store.Data.Repositories.IRepositories
{
    public interface IInquiryHeaderRepository:IRepository<InquiryHeader>
    {
        public void Update(InquiryHeader inquiryHeader);
    }
}