using countMastersTest.infrastructure;
using countMastersTest.infrastructure.constants;
using countMastersTest.infrastructure.data;
using countMastersTest.infrastructure.input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private SwipeDetector _swipeDetector;
    [SerializeField] private SceneSwitcher _sceneSwitcher;

    private GameData _gameData;

    public override void InstallBindings()
    {
        _gameData = getGameData();

        Container.Bind<IInputService>().FromInstance(_swipeDetector).AsSingle().NonLazy();
        Container.Bind<GameData>().FromInstance(_gameData).AsSingle().NonLazy();
        Container.Bind<SceneSwitcher>().FromInstance(_sceneSwitcher).AsSingle().NonLazy();

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            _sceneSwitcher.GoToNextScene();
        }
    }

    private GameData getGameData()
    {
        GameData gameData = DataHandler.Load<GameData>(GameConstants.DATA_NAME);

        if (gameData == null)
        {
            return new GameData();
        }

        return gameData;
    }
}