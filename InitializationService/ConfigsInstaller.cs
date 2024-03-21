using Game.Resourses;
using UnityEngine;
using Zenject;

namespace Game.Initialization
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Installers/ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
    {
        [SerializeField] private GameElementsList _gameElementsList;

        public override void InstallBindings()
        {
            StaticContext.Container.BindInstance(_gameElementsList);
        }
    }
}
