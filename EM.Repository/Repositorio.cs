using EM.Domain.Interface;

namespace EM.Repository
{
    public interface IRepositorio<T> where T : IEntidade
    {
        void Add(T obj);
        void Update(T obj);
        IEnumerable<T> GetAll();

    }
}
