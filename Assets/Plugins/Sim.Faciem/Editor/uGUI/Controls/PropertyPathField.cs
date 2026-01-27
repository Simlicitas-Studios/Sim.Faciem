using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sim.Faciem.uGUI.Editor.Controls
{
    [UxmlElement]
    public partial class PropertyPathField : VisualElement
    {
        // ────────────────────────────────
        // UXML / Binding-facing properties
        // ────────────────────────────────

        [CreateProperty]
        [UxmlAttribute("value")]
        public string Value
        {
            get => _textField.value;
            set => _textField.SetValueWithoutNotify(value);
        }

        /// <summary>
        /// The root type used to offer property path suggestions.
        /// Set this from code (not UXML).
        /// </summary>
        [CreateProperty]
        [UxmlAttribute("referencedDataSource")]
        public string DataSourceType { get; set; }

        /// <summary>
        /// Optional: filter properties by compatible value type.
        /// </summary>
        public Type ExpectedValueType { get; set; }

        // ────────────────────────────────

        private readonly TextField _textField;
        private readonly Button _pickerButton;

        public PropertyPathField()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;

            _textField = new TextField
            {
                isDelayed = true,
                style =
                {
                    flexGrow = 1
                }
            };

            _pickerButton = new Button(OpenPicker)
            {
                text = "⋯",
                tooltip = "Select property path",
                style =
                {
                    width = 22,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };

            Add(_textField);
            Add(_pickerButton);

            // Keep Value in sync with TextField edits
            _textField.RegisterValueChangedCallback(evt =>
            {
                using (ChangeEvent<string>.GetPooled(Value, evt.newValue))
                {
                    Value = evt.newValue;
                }
            });
        }

        private void OpenPicker()
        {
            var remoteDataSourceType = Type.GetType(DataSourceType);
            if (remoteDataSourceType == null)
            {
                Debug.LogWarning("PropertyPathField: DataSourceType is not set.");
                return;
            }

            UnityEditor.PopupWindow.Show(
                _pickerButton.worldBound,
                new PropertyPathPickerPopup(
                    remoteDataSourceType,
                    ExpectedValueType,
                    path => Value = path
                )
            );
        }
    }
}