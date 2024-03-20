using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using FishRoom.Controllers;
using FishRoom.Models;
using FishRoom.TutorialSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace FishRoom.Initialization.Game
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private SceneContext _sceneContext;
        private FishFactory _fishFactory;

        private ILocationController _locationController;
        private PlayerModel _playerModel;
        private VegetationFactory _vegetationFactory;
        private IFishController _fishController;
        private IVegetationController _vegetationController;
        private ITutorialController m_TutorialController;

        private GameObject m_GameScreen;

        [Inject]
        public void Construct(ILocationController locationController,
            PlayerModel playerModel,
            IFishController fishController, 
            IVegetationController vegetationController,
            ITutorialController tutorialController
            )
        {
            _locationController = locationController;
            _playerModel = playerModel;
            _fishController = fishController;
            _vegetationController = vegetationController;
            _fishFactory = _sceneContext.Container.Resolve<FishFactory>();
            _vegetationFactory = _sceneContext.Container.Resolve<VegetationFactory>();
            m_TutorialController = tutorialController;
        }

        private async void Start()
        {
            var location = _playerModel.LocationModels.Find(x => x.Name.Equals(_playerModel.ChoosenLocation));

            _locationController.SetCurrentLocation(location);
            _fishController.SetupLocation();
            _fishController.FillList(location.FishList);
            _vegetationController.SetupLocation();
            _vegetationController.FillList(location.VegetationList);
            
            await _locationController.LaunchLocation();
            await Instantiate("GameScreen.prefab");
            await _fishFactory.InitializationSpawnFish();
            await _vegetationFactory.InitializationSpawnVegetation();
        }

        //Extract to session controller
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) return;

            _locationController.CurrentModel.LastTimeOnline =
                DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);

            _playerModel.Save();
        }

        private async UniTask Instantiate(string path)
        {
            var viewHandle = Addressables.LoadAssetAsync<GameObject>(path);

            if (viewHandle.Status == AsyncOperationStatus.Failed) return;

            var prefab = await viewHandle.Task;
            m_GameScreen = Object.Instantiate(prefab);
            m_GameScreen.name = path;
        }
    }
}