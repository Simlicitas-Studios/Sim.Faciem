using System;
using Sim.Faciem;
using VContainer;

namespace Game.Runtime
{
    public class FaciemVContainerBridge : IDIContainerBridge
    {
        private readonly IObjectResolver _objectResolver;

        public FaciemVContainerBridge(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public T ResolveInstance<T>() where T : class
        {
            return _objectResolver.Resolve<T>();
        }

        public object ResolveInstance(Type type)
        {
            return _objectResolver.Resolve(type);
        }
    }
}