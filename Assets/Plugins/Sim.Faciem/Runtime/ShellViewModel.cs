using Cysharp.Threading.Tasks;

namespace Sim.Faciem
{
    public class ShellViewModel : BaseViewModel
    {
        public ShellViewModel()
        {
            
        }

        protected override async UniTask NavigateTo()
        {
            await Navigation.Navigate(FaciemBridge.RootViewId, WellKnownRegionNames.MainRegion);
        }
    }
}