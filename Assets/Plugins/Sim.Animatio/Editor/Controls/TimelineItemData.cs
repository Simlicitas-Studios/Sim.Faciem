using System;
using System.Collections.Generic;

namespace Sim.Animatio.Editor.Controls
{
    [Serializable]
    public class TimelineItemData
    {
        public int TotalFrames { get; }
        
        public List<TimelineItem> Items { get; }

        public TimelineItemData(int totalFrames, List<TimelineItem> items)
        {
            Items = items;
            TotalFrames = totalFrames;
        }
    }
}