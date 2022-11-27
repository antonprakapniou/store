using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Data.Repositories
{
    public class InquiryHeaderRepository:Repository<InquiryHeader>,IInquiryHeaderRepository
    {
        private readonly AppDbContext _db;

        public InquiryHeaderRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryHeader inquiryHeader)
        {
            _db.InquiryHeaders.Update(inquiryHeader);
        }
    }
}