using Plugins.Sim.Faciem.Runtime.Controls.CommandBinding;
using Sim.Faciem.Commands;
using UnityEditor;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.Runtime.Controls
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
            
            // // Create local Converters.
            // var group = new ConverterGroup("Bool-Visibility");
            //
            // // Converter groups can have multiple converters. This example converts a float to both a color and a string.
            // group.AddConverter((ref bool active) => active 
            //     ?new StyleEnum<DisplayStyle>(DisplayStyle.Flex) 
            //     : new StyleEnum<DisplayStyle>(DisplayStyle.None));
            //
            // group.AddConverter((ref StyleEnum<DisplayStyle>displayStyle) => displayStyle == DisplayStyle.Flex);
            //
            // // Register the converter group in InitializeOnLoadMethod to make it accessible from the UI Builder.
            // ConverterGroups.RegisterConverterGroup(group);
        }
    }
}