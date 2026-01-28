using Sim.Faciem.uGUI;

namespace Game.Runtime.uGUI
{
    public class SampleBoolToStringConverter : SimConverterBehaviour<bool, string>
    {
        public override string Convert(bool from)
        {
            return from ? "True" : "False";
        }
    }
}