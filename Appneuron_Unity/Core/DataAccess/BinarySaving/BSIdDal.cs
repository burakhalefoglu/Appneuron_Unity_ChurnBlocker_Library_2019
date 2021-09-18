using Assets.Appneuron.Core.CoreServices.DataStorageService.Concrete.BinaryType;
using Assets.Appneuron.Core.DataAccessBase;
using Assets.Appneuron.Core.DataModelBase.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataAccess.BinarySaving
{
    public class BSIdDal : BinaryTypeRepositoryBase<CustomerIdModel>, IIdDal
    {
    }
}
