using Sim.Faciem;
using Unity.Properties;
using UnityEngine;

namespace Game.Runtime.UI.MainMenu
{
    public class MainMenuDesignTimeDataContext : DesignTimeDataContext, IMainMenuDataContext
    {
        [SerializeField]
        [DontCreateProperty]
        private string _gameName;

        [CreateProperty] 
        public string GameName {
            get => _gameName;
            set => _gameName = value;
        }
    }
}