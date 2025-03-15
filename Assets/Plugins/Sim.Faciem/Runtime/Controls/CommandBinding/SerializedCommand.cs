using System;
using Sim.Faciem;
using Sim.Faciem.Commands;

namespace Plugins.Sim.Faciem.Runtime.Controls.CommandBinding
{
    [Serializable]
    public class SerializedCommand
    {
        public string Name;

        public Command Command { get; }
        
        public SerializedCommand()
        {
            
        }

        public SerializedCommand(Command command)
        {
            Command = command;
        }
    }
}