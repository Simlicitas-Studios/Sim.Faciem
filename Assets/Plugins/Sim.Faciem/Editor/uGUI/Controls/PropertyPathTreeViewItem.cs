using UnityEditor.IMGUI.Controls;

namespace Sim.Faciem.uGUI.Editor.Controls
{
    internal sealed class PropertyPathTreeViewItem : TreeViewItem
    {
        public string PropertyPath { get; }

        public PropertyPathTreeViewItem(int id, int depth, string displayName, string propertyPath)
            : base(id, depth, displayName)
        {
            PropertyPath = propertyPath;
        }
    }
}