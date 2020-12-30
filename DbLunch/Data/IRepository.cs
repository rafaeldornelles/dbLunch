using DbLunch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbLunch.Data
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> All();
        Task Delete(int id);
        Task<T> Find(int id);
        Task Insert(T modelInsert);
        Task Update(T model);
    }
}