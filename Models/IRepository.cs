using System.Linq.Expressions;

namespace WebUygulamaProje1.Models
{
    public interface IRepository<T> where T : class
    {
        // T -> KitapTuru
        IEnumerable<T> GetAll(); //getall ->  tümünü getir
        T Get(Expression<Func<T, bool>> filtre);
        void Ekle(T entity);
        void Sil(T entity);
        void SilAralik(IEnumerable<T> entities);


    }
}
