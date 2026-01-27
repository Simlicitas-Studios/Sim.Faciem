using TMPro;
using UnityEngine;

namespace Sim.Faciem.uGUI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPBindable : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _textControl;

        [SerializeField]
        private BindableProperty<string> _text;

        private void Reset()
        {
            _textControl = GetComponent<TMP_Text>();
        }
    }
}