using Sim.Faciem.CommandBinding;
using Sim.Faciem.Commands;
using UnityEditor;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    public class ControlConverterRegistry
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void RegisterConverters()
        {
            ConverterGroups.RegisterGlobalConverter((ref Command command) => new SerializedCommand(command));
        }
    }
}