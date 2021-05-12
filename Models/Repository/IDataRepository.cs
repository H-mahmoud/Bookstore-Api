using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repository
{
    public interface IDataRepository<T>
    {
        IEnumerable<T> FindAll();
        T FindById(string Id);
        void Create(T entity);
        bool Update(T entity, string Id);
        bool Delete(string Id);
        void Save();
    }
}
