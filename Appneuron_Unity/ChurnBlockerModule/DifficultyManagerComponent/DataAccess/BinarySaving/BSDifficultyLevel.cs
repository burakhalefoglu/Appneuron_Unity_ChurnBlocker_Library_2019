using Appneuron.Models;
using Assets.Appneuron.Core.CoreServices.DataStorageService.Concrete.BinaryType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.DifficultyManagerComponent.DataAccess.BinarySaving
{
    public class BSDifficultyLevel : BinaryTypeRepositoryBase<DifficultyModel>, IDifficultyLevelDal
    {
    }
}
