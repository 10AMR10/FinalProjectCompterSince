using FinalProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task<T> GetByIdAsync(Expression<Func<T,bool>> match , string[] includes = null);

       

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? match , string[] includes = null);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(int id);
    }
}
