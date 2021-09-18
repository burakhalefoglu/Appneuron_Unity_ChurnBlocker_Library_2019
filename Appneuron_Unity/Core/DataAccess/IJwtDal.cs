using Assets.Appneuron.Core.CoreServices.DataStorageService.Abstract;
using Assets.Appneuron.Core.DataModel.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.DataAccess
{
    public interface IJwtDal : IRepositoryService<TokenDataModel>
    {
    }
}
