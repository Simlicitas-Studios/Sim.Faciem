using System;

namespace Plugins.Sim.Faciem.Editor.DI
{
    public struct ServiceRegistration
    {
        public Type InstanceType { get; }
        
        public bool IsSingleton { get; }

        private ServiceRegistration(Type instanceType, bool isSingleton)
        {
            InstanceType = instanceType;
            IsSingleton = isSingleton;
        }

        public static ServiceRegistration Singleton(Type instanceType)
        {
            return new ServiceRegistration(instanceType, true);
        }
        
        public static ServiceRegistration Transient(Type instanceType)
        {
            return new ServiceRegistration(instanceType, false);
        }
    }
}