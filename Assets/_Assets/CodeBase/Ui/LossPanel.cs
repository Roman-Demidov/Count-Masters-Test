using System;
using UnityEngine;
using UnityEngine.UI;

namespace countMastersTest
{
    public class LossPanel : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action onRestartGame;

        private void Start()
        {
            _button.onClick.AddListener(hide);
        }

        public void show()
        {
            gameObject.SetActive(true);
        }

        public void hide()
        {
            onRestartGame?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
