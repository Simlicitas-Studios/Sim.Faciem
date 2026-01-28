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
        
        private void Awake()
        {
            _sampleText
                .Subscribe(newText => SampleText = newText)
                .AddTo(this);
        }
    }
}