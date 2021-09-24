using System;

namespace Appneuron.Zenject
{
    public interface ILateDisposable
    {
        void LateDispose();
    }
}
