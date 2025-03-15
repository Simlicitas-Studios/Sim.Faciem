using Sim.Faciem;
using Sim.Faciem.Commands;

namespace Plugins.Sim.Faciem.Editor
{
    public interface IDemoEditorWindowDataContext : IDataContext
    {
        Command NextView { get; set; }
        Command PreviousView { get; set; }
    }
}