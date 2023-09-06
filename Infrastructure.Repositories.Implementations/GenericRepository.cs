using Domain.Entities;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Abstractions;

namespace Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DateBaseContext _context;
        private readonly DbSet<T> _dbSet;

        #region public methods and constructor
        public GenericRepository(DateBaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<T> CreateAsync(T entity)
        {
            return await CrudHelperAsync(entity, entity != null, TypeCrud.ADD);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> IsExistByIdAsync(int id)
        {
            try
            {
                if (await _dbSet.AnyAsync(x => x.Id == id))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var flag = await IsExistByIdAsync(entity.Id);
            return await CrudHelperAsync(entity, flag, TypeCrud.UPDATE);
        }

        public async Task<T> RemoveAsync(T entity)
        {
            var flag = await IsExistByIdAsync(entity.Id);
            return await CrudHelperAsync(entity, flag, TypeCrud.DELETE);
        }
        #endregion

        #region private methods
        private async Task<T> CrudHelperAsync(T entity, bool flag, TypeCrud typeOp)
        {
            await using var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                if (flag)
                {
                    switch (typeOp)
                    {
                        case TypeCrud.ADD:
                            _dbSet.Add(entity);
                            break;
                        case TypeCrud.DELETE:
                            _context.Remove(entity);
                            break;
                        case TypeCrud.UPDATE:
                            _dbSet.Entry(entity).State = EntityState.Modified;
                            break;
                        default:
                            break;
                    }
                    await _context.SaveChangesAsync();
                    await tran.CommitAsync();
                    return entity;
                }
                else
                {
                    tran.Rollback();
                    throw new Exception("Объект не существует");
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private enum TypeCrud
        {
            ADD,
            DELETE,
            UPDATE
        }
        #endregion

    }
}
