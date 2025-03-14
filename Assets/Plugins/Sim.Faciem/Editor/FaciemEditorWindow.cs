using System;
using Cysharp.Threading.Tasks;
using Plugins.Sim.Faciem.Editor.DI;
using Plugins.Sim.Faciem.Editor.Navigation;
using Sim.Faciem;
using Sim.Faciem.Internal;
using UnityEditor;
using UnityEngine;

namespace Plugins.Sim.Faciem.Editor
{
    public abstract class FaciemEditorWindow : EditorWindow, IRegionSetup
    {
        [SerializeField]
        private RegionNameDefinition _windowRegionName;

        [SerializeField]
        private EditorViewIdAsset _initialViewId;

        internal RegionManager RegionManager = new();
        
        protected RegionName WindowRegionName => _windowRegionName.Name;
        
        protected IEditorWindowNavigationService Navigation { get; private set; }
        
        private async UniTaskVoid CreateGUI()
        {
            rootVisualElement.dataSource = this;
            var navigationService = EditorInjector.Instance.ResolveInstance<INavigationService>();
            Navigation = new EditorWindowNavigationService(this, navigationService);
            
            var region = new Region
            {
                RegionName = _windowRegionName,
                style =
                {
                    flexGrow = 1
                }
            };

            rootVisualElement.Add(region);
            region.RegisterDirect(this);

            await Navigation.Navigate(_initialViewId.ViewId, WindowRegionName);

            await NavigateTo();
        }

        protected virtual UniTask NavigateTo()
        {
            return UniTask.CompletedTask;
        }

        private async UniTaskVoid OnDisable()
        {
            await NavigateAway();
        }
        
        protected virtual UniTask NavigateAway()
        {
            return UniTask.CompletedTask;
        }

        public void AddRegion(IRegion region)
        {
            RegionManager.AddRegion(region);
        }
    }
}