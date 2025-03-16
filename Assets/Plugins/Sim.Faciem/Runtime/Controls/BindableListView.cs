using System.Collections;
using System.Linq;
using R3;
using Sim.Faciem.ListBinding;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    [UxmlElement]
    public partial class BindableListView : ListView
    {
        private SerializedListReference _itemSource;

        [UxmlAttribute, CreateProperty]
        public SerializedListReference ItemSource
        {
            get => _itemSource;
            set
            {
                _itemSource = value;
                SetDataBinding();
            }
        }
        
        [UxmlAttribute, CreateProperty]
        public int SelectedIndex
        {
            get;
            set;
        }

        protected override void HandleEventTrickleDown(EventBase evt)
        {
            if (evt is AttachToPanelEvent)
            {
                SetDataBinding();
            }
        }

        private void SetDataBinding()
        {
            if (TryGetBinding(nameof(ItemSource), out var itemSourceBinding)
                && itemSourceBinding is DataBinding dataBinding)
            {
                SetBinding(nameof(itemsSource), new DataBinding
                {
                    dataSourcePath = dataBinding.dataSourcePath
                });    
            }

            if (TryGetBinding(nameof(SelectedIndex), out var selectedIndexBinding)
                && selectedIndexBinding is DataBinding selectedIndexDataBinding)
            {
                SetBinding(nameof(selectedIndex), new DataBinding
                {
                    dataSourcePath = selectedIndexDataBinding.dataSourcePath,
                    bindingMode = selectedIndexDataBinding.bindingMode,
                    updateTrigger = selectedIndexDataBinding.updateTrigger
                });   
            }
        }
    }
}