using System;
using UnityEngine;

namespace Sim.Faciem.uGUI
{
    [Serializable]
    public class BindableProperty<T> : IBindableProperty
    {
        [SerializeField]
        private T _value;

        public SimBindingInfo BindingInfo;
        
        public T Value { get =>  _value; set => _value = value; }

        Type IBindableProperty.BoundType => typeof(T);

        SimBindingInfo IBindableProperty.BindingInfo
        {
            get => BindingInfo;
            set => BindingInfo = value;
        }
    }
}