using Sim.Faciem;
using VContainer.Unity;

namespace Game.Runtime
{
    public class FaciemSetupStartup : IStartable
    {
        public FaciemSetupStartup(IDIContainerBridge containerBridge)
        {
            FaciemBridge.ContainerBridge = containerBridge;
        }
        
        public void Start()
        {
            
            
        }
    }
}