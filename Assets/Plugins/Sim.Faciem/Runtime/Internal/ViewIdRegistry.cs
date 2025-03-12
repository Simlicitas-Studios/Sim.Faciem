using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Plugins.Sim.Faciem.Editor;
using UnityEngine.AddressableAssets;

namespace Sim.Faciem.Internal
{
    internal class ViewIdRegistry : IViewIdRegistry
    {
        private readonly Dictionary<ViewId, ViewIdAsset> _viewIds;

        public ViewIdRegistry()
        {
            _viewIds = ViewIdDiscoveryService.ViewIds;
        }

        public bool TryGetViewId(ViewId viewId, out ViewIdAsset viewIdAsset)
        {
            return _viewIds.TryGetValue(viewId, out viewIdAsset);
        }
    }
}