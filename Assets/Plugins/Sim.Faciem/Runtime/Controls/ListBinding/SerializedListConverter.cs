using UnityEditor.UIElements;

namespace Sim.Faciem.ListBinding
{
    public class SerializedListConverter: UxmlAttributeConverter<SerializedListReference>
    {
        public override SerializedListReference FromString(string value)
        {
            return new SerializedListReference();
        }
    }
}