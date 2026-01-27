using System;
using Unity.Properties;

namespace Sim.Faciem.uGUI
{
    [Serializable]
    public struct SimBindingInfo
    {
        public MonoScriptReference DataSource;
        
        public PropertyPath PropertyPath;
    }
}