using StokTakip.Shared.Entities.Abstract; //projeye özel tanımladığım arayüzleri kullanmak için
using System;
using System.Collections.Generic; //List IList<T> tanımları için
using System.Linq; //linq sorguları ; where, select, any, count
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Shared.Data.Abstract
{
    public interface IEntityRepository<T> where T : class,IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T,bool>> predicate,params Expression<Func<T, object>>[]includeProperties); //var article =db.Article.Get(x=>x.id==15)
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate =null, params Expression<Func<T, object>>[]  includeProperties);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate ); //belirli bir koşula ait kayıt var mı?
        Task<int> CountAsync(Expression<Func<T, bool>> predicate); // belirli bir koşula ait kaç kayır var?

    }
}
