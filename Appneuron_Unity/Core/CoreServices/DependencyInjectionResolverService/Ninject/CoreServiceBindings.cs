
using Ninject.Modules;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Concrete.WithEffortless;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.DataAccess.BinarySaving;
using Assets.Appneuron.Core.DataAccessBase;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Concrete.RestSharp;
using Assets.Appneuron.Core.DataAccess;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Appneuron.DifficultyManagerComponent.DataAccess.BinarySaving;
using Appneuron.DifficultyManagerComponent.DataAccess;

namespace Assets.Appneuron.Core.CoreServices.DependencyInjectionResolverService.Ninject
{

    public class CoreServiceBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICryptoServices>().To<EfforlessCryptoServices>().InSingletonScope();
            Bind<IRestClientServices>().To<RestSharpServices>().InSingletonScope();
            Bind<IIdDal>().To<BSIdDal>().InSingletonScope();
            Bind<IJwtDal>().To<BSJwtDal>().InSingletonScope();
            Bind<IKafkaMessageBroker>().To<KafkaMessageHelper>().InSingletonScope();
            Bind<IDifficultyLevelDal>().To<BSDifficultyLevel>().InSingletonScope();
        }
    }
}
