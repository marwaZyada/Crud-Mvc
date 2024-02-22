using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    

      public class DepartmentRepository :GenericRepository<Department>,IDepartmentRepository
    {
        //private readonly CompanyDBContext dbContext;
        public DepartmentRepository(CompanyDBContext dBContext):base(dBContext)
        {
            //this.dbContext = dBContext;

        }
       
    }
}
