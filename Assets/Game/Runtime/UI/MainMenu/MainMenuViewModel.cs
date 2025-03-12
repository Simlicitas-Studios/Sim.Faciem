using System;
using Cysharp.Threading.Tasks;
using R3;
using Sim.Faciem;
using Unity.Properties;

namespace Game.Runtime.UI.MainMenu
{
    [Serializable]
    public class MainMenuViewModel : ViewModel, IMainMenuDataContext
    {
        private const string OriginalGameName = "Runtime Simplicitas Faciem";

        [CreateProperty]
        public string GameName { get; set; } = OriginalGameName;

        private int _counter = 0;
        
        protected override UniTask NavigateTo()
        {
            Disposables.Add(
                Observable.Interval(TimeSpan.FromSeconds(1), UnityTimeProvider.Update)
                    .Subscribe(_ =>
                    {
                        GameName = OriginalGameName + $" {_counter++}";
                    }));
            
            return base.NavigateTo();
        }
    }
}