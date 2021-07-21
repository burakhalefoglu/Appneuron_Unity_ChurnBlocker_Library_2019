using Appneuron.DifficultyManagerComponent.DataAccess;
using Appneuron.DifficultyManagerComponent.DataAccess.BinarySaving;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Concrete.RestSharp;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Core.CoreServices
{
    public class DependencyResolvers : NinjectModule
    {
        public override void Load()
        {
            Bind<IRestClientServices>().To<RestSharpServices>().InSingletonScope();
            Bind<IDifficultyLevelDal>().To<BSDifficultyLevel>().InSingletonScope();
        }
        
    }
}
