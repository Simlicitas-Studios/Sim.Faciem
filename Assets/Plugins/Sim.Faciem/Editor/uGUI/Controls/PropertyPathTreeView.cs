using System;
using UnityEditor.IMGUI.Controls;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

namespace Sim.Faciem.uGUI.Editor.Controls
{
    internal sealed class PropertyPathTreeView : TreeView
    {
        private readonly Type _rootType;
        private readonly Type _expectedValueType;
        private readonly Action<string> _onSelected;

        public PropertyPathTreeView(
            TreeViewState state,
            Type rootType,
            Type expectedValueType,
            Action<string> onSelected)
            : base(state)
        {
            _rootType = rootType;
            _expectedValueType = expectedValueType;
            _onSelected = onSelected;

            Reload();
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            return 30;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item;

            // Use the full row rect
            Rect rowRect = args.rowRect;

            // Optional: indent manually if you want tree structure
            rowRect.x += 6;

            // Create a centered style based on EditorStyles.label
            GUIStyle centeredStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft
            };

            // Draw the label
            EditorGUI.LabelField(rowRect, item.displayName, centeredStyle);
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1 };
            int id = 1;

            BuildProperties(
                root,
                ref id,
                _rootType,
                new PropertyPath());

            return root;
        }

        private void BuildProperties(
            TreeViewItem parent,
            ref int id,
            Type type,
            PropertyPath path)
        {
            foreach (var property in PropertyContainerCompat.GetProperties(type))
            {
                var childPath = path.Append(property.Name);
                var valueType = property.DeclaredValueType();

                bool compatible =
                    _expectedValueType == null ||
                    _expectedValueType.IsAssignableFrom(valueType);

                var item = new PropertyPathTreeViewItem(
                    id++,
                    parent.depth + 1,
                    property.Name,
                    childPath.ToString());

                if (compatible)
                {
                    parent.AddChild(item);   
                }

                if (PropertyContainerCompat.HasProperties(valueType))
                {
                    BuildProperties(item, ref id, valueType, childPath);
                }
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            var item = FindItem(id, rootItem);
            if (item is PropertyPathTreeViewItem pathItem)
            {
                _onSelected?.Invoke(pathItem.PropertyPath);
            }
        }
    }

}