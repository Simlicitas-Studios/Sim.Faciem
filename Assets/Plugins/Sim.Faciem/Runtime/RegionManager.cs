﻿using System.Collections.Generic;
using Bebop.Monads;

namespace Sim.Faciem
{
    public class RegionManager
    {
        private readonly Dictionary<RegionName, IRegion> _regions = new();
        
        public Maybe<RegionManager> Parent { get; }

        public RegionManager()
        {
            Parent = Maybe.Nothing<RegionManager>();
        }
        
        public RegionManager(RegionManager parent)
        {
            Parent = Maybe.From(parent);
        }

        public void AddRegion(IRegion region)
        {
            _regions.Add(region.RegionName, region);
        }

        public bool TryFindRegion(RegionName regionName, out IRegion region)
        {
            return _regions.TryGetValue(regionName, out region);
        }
    }
}