using Sim.Faciem.uGUI;

namespace Game.Runtime.uGUI
{
    public class SampleIntToStringConverter : SimConverterBehaviour<int, string>
    {
        public override string Convert(int from)
        {
            return from switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                _ => "any"
            };
        }
    }
}