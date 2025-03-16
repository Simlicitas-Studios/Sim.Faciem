using Cysharp.Threading.Tasks;
using Plugins.Sim.Faciem.Editor.DI;
using Plugins.Sim.Faciem.Editor.Navigation;
using Plugins.Sim.Faciem.Shared;
using R3;
using Sim.Faciem;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.Editor
{
    public abstract class FaciemToolbarOverlay : ToolbarOverlay, IRegionManagerOwner
    {
        private readonly RegionManager _regionManager;

        RegionManager IRegionManagerOwner.RegionManager => _regionManager;

        protected IEditorToolNavigationService Navigation { get; }

        protected abstract RegionName RegionName { get; }
        
        
        protected FaciemToolbarOverlay()
        {
            _regionManager = new RegionManager();
            var navigationService = EditorInjector.Instance.ResolveInstance<INavigationService>();
            Navigation = new EditorToolNavigationService(this, navigationService);
        }

        protected virtual VisualElement CreateRootElement()
        {
            return new VisualElement();
        }
        
        public sealed override VisualElement CreatePanelContent()
        {
            var root = CreateRootElement();
            var region = new Region(RegionName);

            var disposables = region.RegisterDisposableBag();
            _regionManager.AddRegion(region);
            disposables.Add(Disposable.Create(() =>
            {
                _regionManager.RemoveRegion(RegionName);
                UniTask.Defer(NavigateAway).Forget();
            }));            
            
            UniTask.Defer(NavigateTo).Forget();

            root.Add(region);
            return root;
        }

        public override void OnCreated()
        {
            var button = new ToolbarButton(TogglePanel)
            {
                text = "Toggle Panel"
            };
            button.name = "toggle-panel-button";
            rootVisualElement.Add(button);
        }

        private void TogglePanel()
        {
            collapsed = !collapsed;
        }
        
        protected virtual UniTask NavigateTo()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask NavigateAway()
        {
            return UniTask.CompletedTask;
        }
    }
}