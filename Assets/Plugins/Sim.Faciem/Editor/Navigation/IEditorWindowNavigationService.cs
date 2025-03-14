﻿using Cysharp.Threading.Tasks;
using Sim.Faciem;

namespace Plugins.Sim.Faciem.Editor.Navigation
{
    public interface IEditorWindowNavigationService
    {
        UniTask Navigate(ViewId viewId, RegionName region);
    }
}