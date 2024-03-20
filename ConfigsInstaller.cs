using FishRoom.Resourses;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Installers/ConfigsInstaller")]
public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller> //TODO move to commands
{
    [SerializeField] private GameElementsList _gameElementsList;

    public override void InstallBindings()
    {
        StaticContext.Container.BindInstance(_gameElementsList);
    }
}