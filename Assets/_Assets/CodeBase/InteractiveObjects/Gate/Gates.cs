using TMPro;
using UnityEngine;

namespace countMastersTest.interactiveObjects.gate
{
    public class Gates : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _leftGate;
        [SerializeField] private GameObject _rightGate;
        [SerializeField] private TextMeshProUGUI _leftGateText;
        [SerializeField] private TextMeshProUGUI _rightGateText;
        [SerializeField] private GateSettings _leftGateAction;
        [SerializeField] private GateSettings _rightGateAction;

        private void OnValidate()
        {
            if(_collider == null) _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _leftGateText.text = getGateText(_leftGateAction);
            _rightGateText.text= getGateText(_rightGateAction);
        }

        private string getGateText(GateSettings gateSettings)
        {
            string text = "";

            switch (gateSettings.action)
            {
                case GateActions.Add: text += "+"; break;
                case GateActions.Multiply: text += "*"; break;
            }

            return text + gateSettings.value;
        }

        public GateSettings getLeftAction()
        {
            _leftGate.SetActive(false);
            _collider.enabled = false;
            return _leftGateAction;
        }

        public GateSettings getRightAction()
        {
            _rightGate.SetActive(false);
            _collider.enabled = false;
            return _rightGateAction;
        }
    }
}
