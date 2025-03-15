using System;
using System.Linq;
using Bebop.Monads;
using Cysharp.Threading.Tasks;
using Sim.Faciem.Internal;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    internal class NavigationService : INavigationService
    {
        private readonly IViewModelConstructionService _viewModelConstructionService;
        private readonly IViewIdRegistry _viewIdRegistry;

        public NavigationService(IViewModelConstructionService viewModelConstructionService, IViewIdRegistry viewIdRegistry)
        {
            _viewModelConstructionService = viewModelConstructionService;
            _viewIdRegistry = viewIdRegistry;
        }
        
        public async UniTask NavigateTo(RegionManager regionManager, ViewId viewId, RegionName regionName)
        {
            var maybeRegion = TryFindRegion(regionManager, regionName) as IMaybe<IRegion>;
            
            if (!maybeRegion.HasValue
                || maybeRegion.Value.ActiveViews.Contains(viewId)
                || !_viewIdRegistry.TryGetViewId(viewId, out var viewAsset))
            {
                return;
            }

            var region = maybeRegion.Value;

            if (region.SupportMultipleViews)
            {
                //TODO: Add support for multi view regions
                return;
            }

            var activeViewsCopy = region.ActiveViews.ToList();
            foreach (var currentActiveViewId in activeViewsCopy)
            {
                if (!region.TryGetView(currentActiveViewId, out var oldView)
                    || oldView.dataSource is not BaseViewModel baseViewModel)
                {
                    return;
                }
                
                region.DeactivateView(currentActiveViewId);
                await baseViewModel.NavigateAwayInternal();
            }

            // Active new View 
            BaseViewModel viewModel;
            
            if(region.TryGetView(viewAsset.ViewId, out var view))
            {
                viewModel = view.dataSource as BaseViewModel;
            }
            else
            {
                view = viewAsset.View.Instantiate();
                view.style.flexGrow = 1;

                viewModel = _viewModelConstructionService.CreateInstance(viewAsset.DataContext.Script.GetClass());
                
                var regions = view.Query<Region>().ToList();

                foreach (var innerRegion in regions)
                {
                    innerRegion.RegisterDirect(viewModel);   
                }
                
                region.AddView(viewId, view);
                view.dataSource = viewModel;
            }
            
            region.ActivateView(viewId);

            if (viewModel == null)
            {
                throw new InvalidOperationException("View has wrong Data Source");
            }

            viewModel.RegionManager.Parent = regionManager;
            await viewModel.NavigateToInternal();
        }

        private Maybe<IRegion> TryFindRegion(RegionManager regionManager, RegionName regionName)
        {
            if (!regionManager.TryFindRegion(regionName, out var region))
            {
                return regionManager.Parent
                    .Map(parent => TryFindRegion(parent, regionName));
            }
            
            return Maybe.From(region);
        }
    }
}