using System;
using Plugins.Sim.Faciem.Shared;
using R3;
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

        [CreateProperty]
        [UxmlAttribute]
        public string ExpectedValueType { get; set; }

        // ────────────────────────────────

        private readonly TextField _textField;

        public PropertyPathField()
        {
            var disposables = this.RegisterDisposableBag();
            
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

            Add(_textField);

            disposables.Add(
                _textField.ObserveChanges()
                    .Subscribe(newText => Value = newText));
            
            disposables.Add(
                _textField.FocusInAsObservable()
                    .Subscribe(_ => OpenPicker()));
        }

        private void OpenPicker()
        {
            var remoteDataSourceType = Type.GetType(DataSourceType);
            if (remoteDataSourceType == null)
            {
                Debug.LogWarning("PropertyPathField: DataSourceType is not set.");
                return;
            }

            var expectedValueType = Type.GetType(ExpectedValueType);
            if (expectedValueType == null)
            {
                Debug.LogWarning("PropertyPathField: ExpectedValueType is not set.");
                return;
            }
            
            try
            {
                UnityEditor.PopupWindow.Show(
                    _textField.worldBound,
                    new PropertyPathPickerPopup(
                        new Vector2(_textField.resolvedStyle.width, 400),
                        remoteDataSourceType,
                        expectedValueType,
                        path =>
                        {
                            Value = path;
                            NotifyPropertyChanged(new BindingId(nameof(Value)));
                        })
                    
                );
            }
            catch (ExitGUIException)
            {
                // Ignore
            }

        }
    }
}