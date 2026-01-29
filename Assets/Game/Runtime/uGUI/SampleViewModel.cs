using System;
using R3;
using Sim.Faciem.uGUI;
using Unity.Properties;
using UnityEngine;

namespace Game.Runtime.uGUI
{
    public class SampleViewModel : SimDataSourceMonoBehaviour
    {
        [SerializeField, DontCreateProperty]
        private SerializableReactiveProperty<string> _sampleText;
        
        [CreateProperty]
        public string SampleText { get; set; }

        [CreateProperty]
        public bool SampleBool { get; set; }
        
        [CreateProperty]
        public ReactiveProperty<string> SampleTextReactiveProperty => _sampleText;
        
        [CreateProperty]
        public ReadOnlyReactiveProperty<string> SampleTextReadonlyReactiveProperty => _sampleText;
        
        [CreateProperty]
        public Observable<string>  SampleTextObservable => _sampleText;
        
        [CreateProperty]
        public NestedItem DirectNestedItem { get; set; }

        [CreateProperty]
        public ReactiveProperty<NestedItem> NestedReactiveItem { get; set; } = new();
        
        private void Awake()
        {
            _sampleText
                .Subscribe(newText => SampleText = newText)
                .AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select((_, x) => x)
                .Subscribe(counter => NestedReactiveItem.Value = new NestedItem($"Seconds Elapsed: {counter}"))
                .AddTo(this);

            NestedReactiveItem
                .Where(item => item != null)
                .Subscribe(item => _sampleText.Value = item.TestPath)
                .AddTo(this);
        }
    }
}