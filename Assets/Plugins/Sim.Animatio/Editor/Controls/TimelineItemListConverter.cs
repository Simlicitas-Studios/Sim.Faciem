using System.Collections.Generic;
using UnityEditor.UIElements;

namespace Sim.Animatio.Editor.Controls
{
    internal class TimelineItemListConverter : UxmlAttributeConverter<TimelineItemData>
    {
        public override TimelineItemData FromString(string value)
        {
            return new TimelineItemData(0, new List<TimelineItem>());
        }
    }
}