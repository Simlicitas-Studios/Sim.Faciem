
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Sim.Faciem.uGUI.Editor.Controls
{
    public sealed class PropertyPathPickerPopup : PopupWindowContent
    {
        private readonly Type _rootType;
        private readonly Type _expectedValueType;
        private readonly Action<string> _onSelected;

        private TreeViewState _treeState;
        private PropertyPathTreeView _treeView;

        public PropertyPathPickerPopup(
            Type rootType,
            Type expectedValueType,
            Action<string> onSelected)
        {
            _rootType = rootType;
            _expectedValueType = expectedValueType;
            _onSelected = onSelected;
        }

        public override Vector2 GetWindowSize()
            => new(320, 420);

        public override void OnOpen()
        {
            _treeState ??= new TreeViewState();
            _treeView = new PropertyPathTreeView(
                _treeState,
                _rootType,
                _expectedValueType,
                _onSelected);
        }

        public override void OnGUI(Rect rect)
        {
            _treeView.OnGUI(rect);
        }
    }

}