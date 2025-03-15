using UnityEditor.UIElements;

namespace Sim.Faciem.CommandBinding
{
    public class SerializedCommandConverter : UxmlAttributeConverter<SerializedCommand>
    {
        public override SerializedCommand FromString(string value)
        {
            return new SerializedCommand
            {
                Name = value
            };
        }
    }
}