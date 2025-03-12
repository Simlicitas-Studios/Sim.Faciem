using UnityEngine;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    public class ViewIdAsset : ScriptableObject
    {
        public ViewId ViewId;
        
        public VisualTreeAsset View;

        [MonoScriptReferenceFilter(typeof(IDataContext))]
        public MonoScriptReference DataContext;

        [MonoScriptReferenceDependentFilter(nameof(DataContext))]
        [MonoScriptReferenceFilter(typeof(ViewModel))]
        public MonoScriptReference ViewModel;
    }
}
