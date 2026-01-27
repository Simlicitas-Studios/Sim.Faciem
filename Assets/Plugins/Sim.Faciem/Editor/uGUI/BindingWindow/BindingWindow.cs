using Plugins.Sim.Faciem.Editor;
using R3;
using UnityEngine;

namespace Sim.Faciem.uGUI.Editor.BindingWindow
{
    public class BindingWindow : FaciemEditorWindow
    {
        private Subject<Unit> _onDestorySubject = new();
        
        public Observable<Unit> Closed => _onDestorySubject;
        
        public BindingWindow()
        {
            titleContent = new GUIContent("Binding Setup");
        }

        private void OnDestroy()
        {
            _onDestorySubject.OnNext(Unit.Default);
        }
    }
}