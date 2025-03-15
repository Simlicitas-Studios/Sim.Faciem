using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sim.Faciem;
using Sim.Faciem.Commands;
using Unity.Properties;
using R3;

namespace Plugins.Sim.Faciem.Editor
{
    public class DemoEditorWindowViewModel : ViewModel, IDemoEditorWindowDataContext
    {
        private readonly List<ViewId> _views = new(){
            WellKnownDemoViewIds.FirstDemoView,
            WellKnownDemoViewIds.SecondDemoView,
        };
        
        private readonly ReactiveProperty<int> _currentViewIndex;
        
        [CreateProperty]
        public Command NextView { get; set; }

        [CreateProperty]
        public Command  PreviousView { get; set; }
        
        public DemoEditorWindowViewModel()
        {
            _currentViewIndex = new ReactiveProperty<int>(0);
            
            NextView = Command.ExecuteAsync(NavigateToNextView)
                .WithCanExecute(_currentViewIndex
                    .Select(index => index + 1 < _views.Count));
            
            PreviousView = Command.ExecuteAsync(NavigateToPreviousView)
                .WithCanExecute(_currentViewIndex
                    .Select(index => index - 1 >= 0));
        }

        
        protected override async UniTask NavigateTo()
        {
            await ShowCurrentView();
        }

        
        
        private async UniTask NavigateToNextView(CancellationToken ct)
        {
            _currentViewIndex.Value += 1;
            await ShowCurrentView();
        }

        private async UniTask NavigateToPreviousView(CancellationToken ct)
        {
            _currentViewIndex.Value -= 1;
            await ShowCurrentView();
        }

        private async Task ShowCurrentView()
        {
            await Navigation.Navigate(_views[_currentViewIndex.CurrentValue], WellKnownDemoRegions.DemoWindow_Content);
        }
    }
}