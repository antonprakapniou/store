using Store.Data.Repositories.IRepositories;
using Store.Models;

namespace Store.Data.Repositories
{
    public class InquiryDetailsRepository:Repository<InquiryDetails>,IInquiryDetailsRepository
    {
        private readonly AppDbContext _db;

        public InquiryDetailsRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryDetails inquiryDetails)
        {
            _db.Update(inquiryDetails);
        }
    }
}