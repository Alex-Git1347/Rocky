using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models.Models;
using Rocky.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.DataAccess.Repository
{
    public class InquiryHeaderRepository : Repository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly ApplicationDBContext _db;

        public InquiryHeaderRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryHeader inquiryHeader)
        {
            _db.InquiryHeader.Update(inquiryHeader);
        }
    }
}
