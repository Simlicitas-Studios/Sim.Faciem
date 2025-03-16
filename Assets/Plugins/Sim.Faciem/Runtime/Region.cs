using System.Collections.Generic;
using Bebop.Monads;
using Plugins.Sim.Faciem.Shared;
using R3;
using Sim.Faciem.Internal;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    [UxmlElement]
    public partial class Region : VisualElement, IRegion
    {
        private readonly Maybe<RegionName> _regionName;
        private readonly Dictionary<ViewId, VisualElement> _views;
        private readonly List<ViewId> _currentActiveViews;
        private bool _isDetached;

        [UxmlAttribute] 
        public RegionNameDefinition RegionName { get; set; }

        [UxmlAttribute]
        public bool SupportMultipleViews { get; set; }
        
        public IReadOnlyList<ViewId> ActiveViews => _currentActiveViews;
        
        public Region(RegionName regionName)
            : this()
        {
            _regionName = Maybe.From(regionName);
        }
        
        public Region()
        {
            _currentActiveViews = new List<ViewId>();
            _views = new Dictionary<ViewId, VisualElement>();
        }

        public bool TryGetView(ViewId viewId, out VisualElement view)
        {
            return _views.TryGetValue(viewId, out view);
        }

        public void AddView(ViewId viewId, VisualElement view)
        {
            if (_views.TryAdd(viewId, view))
            {
                Add(view);
                return;
            }
            
            throw new System.InvalidOperationException("View already added");
        }

        public void ActivateView(ViewId viewId)
        {
            if (!_views.TryGetValue(viewId, out var view))
            {
                return;
            }
            
            view.Show();
            _currentActiveViews.Add(viewId);
        }

        public void DeactivateView(ViewId viewId)
        {
            if (!_views.TryGetValue(viewId, out var view))
            {
                return;
            }
            
            view.Hide();
            _currentActiveViews.Remove(viewId);
        }

        public void DeactivateAllViews()
        {
            foreach (var activeViewId in _currentActiveViews)
            {
                if (!_views.TryGetValue(activeViewId, out var view))
                {
                    continue;
                }
                view.Hide();
            }
            _currentActiveViews.Clear();
        }
        
        RegionName IRegion.RegionName => _regionName.OrElse(() => RegionName.Name);

        internal void RegisterDirect(IRegionSetup regionSetup)
        {
            regionSetup.AddRegion(this);
        }
    }
}