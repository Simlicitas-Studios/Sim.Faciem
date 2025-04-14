using System.Collections.Generic;

namespace Sim.Animatio.Editor.Controls
{
    public class TimelineItem
    {
        public string ItemName { get; }
        
        public List<int> KeyFrames { get; }

        
        public TimelineItem(string itemName, List<int> keyFrames)
        {
            ItemName = itemName;
            KeyFrames = keyFrames;
        }
    }
}