
using EntityRepository.Context;
using EntityRepository.Repository;
using Entitys.BLL.Auxiliary;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Interfaces;
using RepositoryCore.Result;
using System;
using System.Linq;

using System.Linq.Expressions;
namespace Entitys.BLL
{
    public interface IEntityService<TEntity> : IRepositoryCore<TEntity, int>
       where TEntity : class, IEntity<int>
    {
        IQueryable<TEntity> AllAsQueryable { get; }

        PagedResult<TEntity> Paging(PageState state);

        PagedResult<TEntity> Paging(PageState state, Expression<Func<TEntity, bool>> predicate);
        PagedResult<TEntity> GetPaged(IQueryable<TEntity> query, int page, int pageSize);

    }

    public class EntityService<TEntity> : EntityRepository<TEntity>, IEntityService<TEntity>
        where TEntity : class, IEntity<int>
    {
        public EntityService(IDbContext context) : base(context)
        {

        }

        public virtual IQueryable<TEntity> AllAsQueryable => _dbSet.AsQueryable();


        public PagedResult<TEntity> Paging(PageState state)
        {
            var result = GetPaged(AllAsQueryable, state.Skip, state.Take);
            return result;
        }

        public PagedResult<TEntity> Paging(PageState state, Expression<Func<TEntity, bool>> predicate)
        {
            var query = AllAsQueryable.Where(predicate);

            var result = GetPaged(query, state.Skip, state.Take);
            return result;
        }

        public PagedResult<TEntity> GetPaged(IQueryable<TEntity> query,
                                         int page, int pageSize)
        {
            var result = new PagedResult<TEntity>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = page * pageSize;
          //  result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
