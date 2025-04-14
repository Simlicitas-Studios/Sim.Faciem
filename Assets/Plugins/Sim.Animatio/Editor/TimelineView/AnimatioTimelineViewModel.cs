using System.Collections.Generic;
using Sim.Animatio.Editor.Controls;
using Sim.Faciem;
using Unity.Properties;

namespace Sim.Animatio.Editor.TimelineView
{
    public class AnimatioTimelineViewModel : ViewModel<AnimatioTimelineViewModel>, IAnimatioTimelineDataContext
    {

        [CreateProperty]
        public TimelineItemData Items { get; set; }

        public AnimatioTimelineViewModel()
        {
            Items = new TimelineItemData(60, new List<TimelineItem>
            {
                new("Position.x", new List<int> { 1, 5, 10, 20, 50}),
                new("Position.y", new List<int> { 1, 4, 8, 22, 44})
            });
        }
        
    }
}