using System;
using UnityEngine;
using UnityEngine.UI;

namespace countMastersTest
{
    public class WinPanel: MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action startNextLevel;

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
            startNextLevel?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
