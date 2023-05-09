using countMastersTest.infrastructure;
using countMastersTest.infrastructure.input;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private SwipeDetector _swipeDetector;

    public override void InstallBindings()
    {
        Container.Bind<IInputService>().FromInstance(_swipeDetector).AsSingle().NonLazy();
        Container.Bind<Game>().AsSingle().NonLazy();
    }
}