using UnityEngine;

namespace Plugins.Sim.Faciem.Editor.DI
{
    [CreateAssetMenu(fileName = "EditorServiceInstaller", menuName = "Faciem/Editor/Editor Service Installer")]
    public abstract class EditorServiceInstaller : ScriptableObject
    {
        public abstract void Install(IEditorInjector injector);
    }
}