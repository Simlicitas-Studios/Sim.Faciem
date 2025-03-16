﻿using System.Collections;
using R3;
using Sim.Faciem.CommandBinding;
using Sim.Faciem.Commands;
using Sim.Faciem.ListBinding;
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
            // ReactiveProperties
            
            
            // Command
            ConverterGroups.RegisterGlobalConverter((ref Command command) => new SerializedCommand(command));
            
            // List
            ConverterGroups.RegisterGlobalConverter((ref IList list) => new SerializedListReference(list));
        }
    }
}