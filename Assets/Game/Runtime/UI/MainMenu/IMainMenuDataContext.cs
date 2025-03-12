using Sim.Faciem;

namespace Game.Runtime.UI.MainMenu
{
    public interface IMainMenuDataContext : IDataContext
    {
        string GameName { get; }
    }
}