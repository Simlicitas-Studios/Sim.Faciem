﻿using System.Collections.Generic;
using Bebop.Monads;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    public interface IRegion
    {
        bool SupportMultipleViews { get; }
        
        RegionName RegionName { get; }
        
        IReadOnlyList<ViewId> ActiveViews { get; }

        void AddView(ViewId viewId, VisualElement view);
        
        bool TryGetView(ViewId viewId, out VisualElement view);
        
        void ActivateView(ViewId viewId);
        
        void DeactivateView(ViewId viewId);

        void DeactivateAllViews();
    }
}