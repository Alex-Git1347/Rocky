using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.DataAccess.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDBContext _db;

        public InquiryDetailRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryDetail inquiryDetail)
        {
            _db.InquiryDetail.Update(inquiryDetail);
        }
    }
}
