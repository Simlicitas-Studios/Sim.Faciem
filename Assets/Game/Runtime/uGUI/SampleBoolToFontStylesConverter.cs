using Sim.Faciem.uGUI;
using TMPro;

namespace Game.Runtime.uGUI
{
    public class SampleBoolToFontStylesConverter : SimConverterBehaviour<bool, FontStyles>
    {
        public override FontStyles Convert(bool from)
        {
            return from ? FontStyles.Bold : FontStyles.Normal;
        }
    }
}