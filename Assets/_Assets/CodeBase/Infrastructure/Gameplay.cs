using System.Collections.Generic;
using Cinemachine;
using countMastersTest.character;
using countMastersTest.infrastructure;
using countMastersTest.infrastructure.constants;
using countMastersTest.infrastructure.data;
using countMastersTest.infrastructure.input;
using UnityEngine;
using Zenject;

namespace countMastersTest
{
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private StartScreen _startScreen;
        [SerializeField] private InfoHub _infoHub;
        [SerializeField] private WinPanel _winPanel;
        [SerializeField] private LossPanel _lossPanel;
        [SerializeField] private UserCharacter _pfUserCharacter;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private List<Level> _levelList;

        private GameData _gameData;
        private IInputService _inputService;
        private UserCharacter _userCharacter;
        private SceneSwitcher _sceneSwitcher;
        private int _coinPerLevel = 0;

        [Inject]
        private void constructor(GameData gameData, IInputService inputService, SceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
            _inputService = inputService;
            _gameData = gameData;
        }

        private void Start()
        {
            _startScreen.show();
            _startScreen.onPlay += startPlay;

            Application.targetFrameRate = 30;
            enableLevel();
        }

        private void enableLevel()
        {
            int levelIndex = (_gameData.level - 1) < _levelList.Count ? _gameData.level - 1 : UnityEngine.Random.Range(0, _levelList.Count);

            Level level = _levelList[levelIndex];

            level.setActive(true);

            _userCharacter = Instantiate(_pfUserCharacter, level.getUserCharacterPosition(), Quaternion.identity);
            _userCharacter.init(_inputService);
            _camera.Follow = _userCharacter.transform;
            _userCharacter.onDeath += showLossUi;
            _userCharacter.onFinish += showFinishUi;
            _userCharacter.onPickedCoin += addCoin;

        }

        private void showFinishUi()
        {
            _winPanel.show();
            _winPanel.startNextLevel += startNextLevel;
        }

        private void startNextLevel()
        {
            _gameData.level++;
            _gameData.coins += _coinPerLevel;

            DataHandler.save(_gameData, GameConstants.DATA_NAME);

            _winPanel.startNextLevel -= startNextLevel;
            _sceneSwitcher.restartScene();
        }

        private void addCoin(Transform transform)
        {
            _coinPerLevel++;
            _infoHub.addCoint(_coinPerLevel, transform);
        }

        private void startPlay()
        {
            _infoHub.show(_gameData.level.ToString(), 0);
            _userCharacter.startMove();
        }

        private void showLossUi()
        {
            _lossPanel.show();
            _lossPanel.onRestartGame += restartGame;
        }

        private void restartGame()
        {
            _lossPanel.onRestartGame -= restartGame;
            _sceneSwitcher.restartScene();
        }
    }
}
