
using System;
using System.Runtime.CompilerServices;
using R3;
using Sim.Faciem.PropertyObserver;
using UnityEngine.UIElements;

namespace Sim.Faciem
{
    public abstract class ViewModel<T> : BaseViewModel, INotifyBindablePropertyChanged where T : ViewModel<T>
    {
        private event EventHandler<BindablePropertyChangedEventArgs> PropertyChanged;

        internal Observable<BindablePropertyChangedEventArgs> PropertyChangedObs { get; set; }
        
        protected IViewModelPropertyObserver<T> Property { get; }
        
        protected ViewModel()
        {
            PropertyChangedObs = Observable.FromEvent<EventHandler<BindablePropertyChangedEventArgs>, BindablePropertyChangedEventArgs>(
                    x => (_, args) => x(args),
                    x => PropertyChanged += x,
                    x => PropertyChanged -= x);

            Property = new ViewModelPropertyObserver<T>(this as T);
        }
        
        protected void SetProperty<TItem>(ref TItem item, TItem newValue, [CallerMemberName] string name = "")
        {
            if (item.Equals(newValue))
            {
                return;
            }
            
            item = newValue;
            PropertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(name));
        }
        
        event EventHandler<BindablePropertyChangedEventArgs> INotifyBindablePropertyChanged.propertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }
    }
}