using System;

namespace Appneuron.Zenject
{ 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NoReflectionBakingAttribute : Attribute
    {
    }
}
