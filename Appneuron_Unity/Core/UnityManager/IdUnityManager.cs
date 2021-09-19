using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.DataAccess.BinarySaving;
using Assets.Appneuron.Core.DataAccessBase;
using Assets.Appneuron.Core.DataModelBase.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Assets.Appneuron.Core.CoreServices.IdConfigService.IdConfigServices;
using UnityEngine;
using Appneuron.Models;

namespace AppneuronUnity.Core.UnityManager
{
    public class IdUnityManager
    {
        public async Task SaveIdOnLocalStorage()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _bSIdDal = kernel.Get<IIdDal>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                string savePath = CustomerIdPath + ModelNames.IdName;

                if (!File.Exists(savePath))
                {
                    string id = GenerateId();
                    await _bSIdDal.InsertAsync(savePath, new CustomerIdModel
                    {
                        _id = id
                    });

                }
            }
        }
        public string GetPlayerID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public string GenerateId()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _cryptoServices = kernel.Get<ICryptoServices>();
                return _cryptoServices.GetRandomHexNumber(32);

            }

        }
    }
}
