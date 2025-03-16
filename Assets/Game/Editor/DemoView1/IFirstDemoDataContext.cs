using System.Collections;
using System.Collections.Generic;
using Game.Editor.DemoWindow1.Item;
using Sim.Faciem;

namespace Game.Editor.DemoWindow1
{
    public interface IFirstDemoDataContext : IDataContext
    {
        List<ItemModel> Items { get; }
        int SelectedIndex { get; set; }
    }
}