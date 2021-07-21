using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.DataStorageService.Abstract
{
    public interface IRepositoryService<T>
    {

        Task InsertAsync(string filePath, T dataModel);
        Task<T> SelectAsync(string filePath);
        Task DeleteAsync(string filePath);

    }
}
