using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly CompanyDBContext _dbcontext;

        public UnitOfWork(CompanyDBContext dbcontext)
        {
            EmployeeRepository=new EmployeeRepository(dbcontext);
            DepartmentRepository=new DepartmentRepository(dbcontext);
            _dbcontext = dbcontext;
        }
        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get ; set; }

        public async Task<int> Complete()
        =>await _dbcontext.SaveChangesAsync();

        public void Dispose()
        {
           _dbcontext.Dispose();
        }
    }
}
