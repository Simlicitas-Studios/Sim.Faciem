using R3;
using UnityEditor;
using UnityEngine.UIElements;

namespace Sim.Faciem.Converters
{
    internal class SimConverterRegistry
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void RegisterConverters()
        {
            // bool to DisplayStyle
            ConverterGroups.RegisterGlobalConverter((ref bool handle) => handle 
                ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex) 
                : new StyleEnum<DisplayStyle>(DisplayStyle.None));
            
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