using Assets.Appneuron.Core.CoreServices.DataStorageService.Abstract;
using Assets.Appneuron.Core.DataModelBase.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataAccessBase
{
    public interface IIdDal: IRepositoryService<CustomerIdModel>
    {
    }
}
