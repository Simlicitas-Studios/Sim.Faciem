using Sim.Faciem.uGUI;

namespace Game.Runtime.uGUI
{
    public class SampleStringToBoolConverter : SimConverterBehaviour<string, bool>
    {
        public override bool Convert(string from)
        {
            return bool.TryParse(from, out var result) && result;
        }
    }
}