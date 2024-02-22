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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CompanyDBContext DBContext;
        public GenericRepository(CompanyDBContext dBContext)
        {
            DBContext = dBContext;
        }

        public async Task AddAsync(T item)
        {
        await DBContext.AddAsync(item);
            
        }

        public void Delete(T item)
        {
            DBContext.Remove(item);
         
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Employee))
                return (IEnumerable<T>)await DBContext.Employees.Include(e=>e.Department).ToListAsync();
            return await DBContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DBContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
           
           DBContext.Update(item);
          
        }
    }
}
