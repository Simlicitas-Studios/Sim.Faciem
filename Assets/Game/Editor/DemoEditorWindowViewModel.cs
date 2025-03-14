using Cysharp.Threading.Tasks;
using Sim.Faciem;

namespace Plugins.Sim.Faciem.Editor
{
    public class DemoEditorWindowViewModel : ViewModel, IDemoEditorWindowDataContext
    {
        protected override async UniTask NavigateTo()
        {
            await Navigation.Navigate(WellKnownDemoViewIds.First_Demo_Window, WellKnownDemoRegions.DemoWindow_Content);
        }
    }
}