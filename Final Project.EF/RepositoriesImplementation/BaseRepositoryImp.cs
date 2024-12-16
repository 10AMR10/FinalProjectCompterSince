﻿using FinalProject.Core.IRepositories;
using FinalProject.Core.Models;
using FinalProject.EF.Configuration;
using FinalProject.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FinalProject.EF.RepositoriesImplementation
{
    public class BaseRepositoryImp<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepositoryImp(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {

            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> match, string[]? includes = null)
        {
            //var entity = _context.Set<T>().Find(id);
            //if (entity == null)
            //    return null;
            //_context.Entry(entity).State= EntityState.Detached;


            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            //asnoTracking
            var entity = await query.FirstOrDefaultAsync(match);
            if (entity == null)
                return null;

             
            return entity;
        }

        
public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? match =null, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
			if (match != null)
			{
				query = query.Where(match).AsNoTracking();
			}


			return await query.ToListAsync();
			
		}
        public void Update(T entity)
        {
            
           _context.Set<T>().Update(entity);
        
        }

        public void Delete(T entity)
        {
           
            _context.Set<T>().Remove(entity);
            
        }

	}
}
