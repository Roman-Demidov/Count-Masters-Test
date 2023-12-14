using System;
using countMastersTest.infrastructure.data;
using countMastersTest.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace countMastersTest
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private TutorialUI _tutorialUI;
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _coinText;

        private GameData _gameData;

        public event Action onPlay;

        [Inject]
        private void constructor(GameData gameData)
        {
            _gameData = gameData;
        }

        public void show()
        {
            gameObject.SetActive(true);
            _coinText.text = _gameData.coins.ToString();
            _playButton.onClick.AddListener(hide);
            _tutorialUI.show();
        }

        public void hide()
        {
            onPlay?.Invoke();
            _tutorialUI.hide();
            gameObject.SetActive(false);
        }
    }
}
