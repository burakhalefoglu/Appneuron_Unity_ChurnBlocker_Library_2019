using System;

namespace Appneuron.Zenject
{
    [AttributeUsage(AttributeTargets.Parameter
        | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InjectOptionalAttribute : InjectAttributeBase
    {
        public InjectOptionalAttribute()
        {
            Optional = true;
        }
    }
}

