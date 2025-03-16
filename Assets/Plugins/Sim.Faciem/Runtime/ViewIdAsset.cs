using System;
using UnityEditor;
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
        [MonoScriptReferenceFilter(typeof(ViewModel<>))]
        public MonoScriptReference ViewModel;
        
        
        public bool SourceCodeGeneration;

        public MonoScript SourceFile;

#if UNITY_EDITOR

        internal Action<ViewIdAsset> ExecuteCodeGeneration { get; set; }
        
        private void OnValidate()
        {
            ExecuteCodeGeneration?.Invoke(this);
        }

#endif
    }
}
