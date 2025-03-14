using Sim.Faciem;

namespace Plugins.Sim.Faciem.Editor.DI
{
    public interface IEditorInjector : IDIContainerBridge
    {
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;
    }
}