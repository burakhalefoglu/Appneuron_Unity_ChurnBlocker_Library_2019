using Appneuron.Models;
using Assets.Appneuron.Core.CoreServices.DataStorageService.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.DifficultyManagerComponent.DataAccess
{
   public interface IDifficultyLevelDal : IRepositoryService<DifficultyModel>
    {
    }
}
