using Plugins.Sim.Faciem.Runtime.Controls.CommandBinding;
using Plugins.Sim.Faciem.Shared;
using R3;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.Runtime.Controls
{
    [UxmlElement]
    public partial class BindableButton : Button
    {
        private SerializedCommand _command;
        private DisposableBag _commandSubscriptions;

        [UxmlAttribute, CreateProperty]
        public SerializedCommand Command
        {
            get => _command;
            set
            {
                _command = value;
                RegisterCommandCallbacks();
            }
        }

        public BindableButton()
        {
            var lifeTimeDisposables = this.RegisterDisposableBag();
            
            lifeTimeDisposables.Add(Observable.FromEvent(
                x => clickable.clicked += x,
                x => clickable.clicked -= x)
                .Subscribe(_ =>
                {
                    _command?.Command?.Execute(Unit.Default);
                }));
        }

        private void RegisterCommandCallbacks()
        {
            _commandSubscriptions.Dispose();
            _commandSubscriptions = new DisposableBag();
            
            _commandSubscriptions.Add(
                _command.Command.CanExecuteObs
                    .Subscribe(SetEnabled));
            
            _commandSubscriptions.Add(
                _command.Command.IsVisibleObs
                    .Subscribe(isVisible =>
                    {
                        style.display = isVisible 
                            ? DisplayStyle.Flex 
                            : DisplayStyle.None;
                    }));
        }
    }
}