using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDBContext DBContext;
        public EmployeeRepository(CompanyDBContext dBContext):base(dBContext)
        {
            DBContext = dBContext;
        }



        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return DBContext.Employees.Where(e => e.Address == address);
        }

        public IQueryable<Employee> GetEmployeesByName(string name)
        {

            return DBContext.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower())).Include(e=>e.Department);

        }
    }
}
