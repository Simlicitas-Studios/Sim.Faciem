using System.Collections.Generic;
using Game.Editor.DemoWindow1.Item;
using R3;
using Sim.Faciem;
using Unity.Properties;
using UnityEngine;

namespace Game.Editor.DemoWindow1
{
    public class FirstDemoViewModel : ViewModel<FirstDemoViewModel>, IFirstDemoDataContext
    {
        private int _selectedIndex;

        [CreateProperty]
        public List<ItemModel> Items { get; set; }

        [CreateProperty]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public FirstDemoViewModel()
        {
            Items = new List<ItemModel>
            {
                new("Iron Sword", 1.5f, ItemRarity.Common),
                new("Diamond Sword", 22f, ItemRarity.Rare),
                new("Flame Diamond Sword", 122f, ItemRarity.Legendary),
            };
            
            SelectedIndex = 1;
            
            Disposables.Add(
                Property.Observe(x => x.SelectedIndex)
                    .Subscribe(index => Debug.Log($"Selected item at index {index}")));
        }
    }
}