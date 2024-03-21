using FishRoom.Resourses;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Installers/ConfigsInstaller")]
public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
{
    [SerializeField] private GameElementsList _gameElementsList;

    public override void InstallBindings()
    {
        StaticContext.Container.BindInstance(_gameElementsList);
    }
}