using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sim.Faciem.Internal;

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
            if (!regionManager.TryFindRegion(regionName, out var region)
                || region.ActiveViews.Contains(viewId)
                || !_viewIdRegistry.TryGetViewId(viewId, out var viewAsset))
            {
                return;
            }

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
                view = viewAsset.View.Instantiate()[0];

                viewModel = _viewModelConstructionService.CreateInstance(viewAsset.DataContext.Script.GetClass());
                region.AddView(viewId, view);
                view.dataSource = viewModel;
            }
            
            region.ActivateView(viewId);

            if (viewModel == null)
            {
                throw new InvalidOperationException("View has wrong Data Source");
            }
            
            await viewModel.NavigateToInternal();
        }
    }
}