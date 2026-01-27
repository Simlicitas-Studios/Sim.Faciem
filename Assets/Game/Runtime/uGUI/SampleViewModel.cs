using Sim.Faciem;
using Unity.Properties;

namespace Game.Runtime.uGUI
{
    public class SampleViewModel : ViewModel<SampleViewModel>
    {
        [CreateProperty]
        public string SampleText { get; set; }
    }
}