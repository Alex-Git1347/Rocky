using Rocky.Data;
using Rocky.DataAccess.IRepository;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky.DataAccess.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDBContext _db;

        public ApplicationTypeRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationType appType)
        {
            var objFromDb = base.FirstOrDefault(u => u.AppTypeId == appType.AppTypeId);
            if (objFromDb != null)
            {
                objFromDb.Name = appType.Name;
            }
        }
    }
}
