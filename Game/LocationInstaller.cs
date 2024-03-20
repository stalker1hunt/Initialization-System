using UnityEngine;
using Zenject;

namespace FishRoom.Initialization.Game
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private FishFactory fishFactory;
        [SerializeField] private VegetationFactory vegetationFactory;
        [SerializeField] private SceneContext _sceneContext;

        public override void InstallBindings() //TODO need write mechanism work scene context and project context
        {
            _sceneContext.Container.Bind<FishFactory>().FromInstance(fishFactory);
            _sceneContext.Container.Bind<VegetationFactory>()
                .FromInstance(vegetationFactory);
        }
    }
}