using Cysharp.Threading.Tasks;
using Sim.Faciem;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.Editor.DemoSceneOverlay
{
    
    [Overlay(typeof(SceneView), "Faciem Demo Overlay", true)]
    public class DemoSceneToolbarOverlay : FaciemToolbarOverlay
    {
        protected override RegionName RegionName => RegionName.From("Demo/Scene Overlay");

        protected override VisualElement CreateRootElement()
        {
            return new VisualElement
            {
                style =
                {
                    maxWidth = 600,
                    maxHeight = 600,
                }
            };
        }

        protected override async UniTask NavigateTo()
        {
            await Navigation.Navigate(WellKnownDemoViewIds.FirstDemoView, RegionName);
        }
    }
}