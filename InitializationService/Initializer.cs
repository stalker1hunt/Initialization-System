using CrazyGames;
using Cysharp.Threading.Tasks;
using Game.BoosterSystem;
using Game.Initialization.Commands;
using Game.Models;
using Game.Resourses;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;
#endif

namespace Game.Initialization
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _slisderText;
        [SerializeField] private GameObject _errorPopup;
        [SerializeField] private GameElementsList _gameElementsList;
        [SerializeField] private Image _logoImage;

        private InitializationBootstrapper _initializationBootstrapper;
        private float _progress;
        private float _progressLogo;
        private float _loadingValueStub;
        
        private async void Awake()
        {
            Application.targetFrameRate = 60;
            await Init();
        }

        private void FixedUpdate()
        {
            UpdateProgressSlider();
        }

        public void Exit()
        {
            Application.Quit();
        }

        private async UniTask Init()
        {
            var initializationContext = new InitializationContext
            {
                Container = StaticContext.Container
            };

            initializationContext.Container.BindInstance(_gameElementsList);

            _initializationBootstrapper = new InitializationBootstrapper(initializationContext);

#if UNITY_ANDROID && UNITY_IOS
            _initializationBootstrapper.AddPreInitializationCommand<UnityServiceInitializationCommand>();
            _initializationBootstrapper.AddPreInitializationCommand<FirebaseInitializationCommand>();
#endif
            
            _initializationBootstrapper.AddPreInitializationCommand<AdvertisingInitializationCommand>();
            _initializationBootstrapper.AddPreInitializationCommand<PlayerModelInitializationCommand>();
         
            _initializationBootstrapper.AddPreInitializationCommand<CloudSyncInitializationCommand>();
            _initializationBootstrapper.AddInitializationCommand<BoosterInitializationCommand>();

            _initializationBootstrapper.AddInitializationCommand<EconomyInitializationCommand>();
            _initializationBootstrapper.AddInitializationCommand<GameInitializationCommand>();
            _initializationBootstrapper.AddInitializationCommand<StoreInitializationCommand>();
            _initializationBootstrapper.AddInitializationCommand<TutorialInitializationCommand>();
            
            _initializationBootstrapper.AddInitializationCommand<CheatPanelCommand>();
            
            _initializationBootstrapper.AddInitializationCommand<AnalyticInitializationCommand>();

#if UNITY_WEBGL
            _initializationBootstrapper.AddInitializationCommand<CrazySDKInitializationCommand>();
#endif

#if UNITY_IOS
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            Version currentVersion = new Version(Device.systemVersion); 
            Version ios14 = new Version("14.5"); 
           
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && currentVersion >= ios14)
            {
                Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
            else
            {
                Debug.LogWarning("Unity iOS Support: Tried to request iOS App Tracking Transparency native dialog, " +
                             "but the current platform is not iOS.");
            }
#endif
            
            await InitBootstrapper(initializationContext);
        }

        private async UniTask InitBootstrapper(InitializationContext initializationContext)
        {
            await _initializationBootstrapper.InitAsync();
            await UpdateSliderValueStub();
            await UniTask.Delay(1000);
            SceneManager.LoadSceneAsync(initializationContext.Container.Resolve<PlayerModel>().ChoosenLocation);
            CrazyEvents.Instance.GameplayStart();
        }

        private async UniTask UpdateSliderValueStub()
        {
            for (float i = 0; i < 1.1f; i+=0.1f)
            {
                _loadingValueStub = i;
                await UniTask.Delay(100);
            }
        }

        private void UpdateProgressSlider()
        {
            if (_initializationBootstrapper == null) return;

            _slisderText.text = (int)(_loadingValueStub * 100f) + "%";
            
            _slider.value = Mathf.SmoothDamp(_slider.value, _loadingValueStub,
                ref _progress, 0.2f);
            _logoImage.fillAmount = Mathf.SmoothDamp(_logoImage.fillAmount, _loadingValueStub,
                ref _progressLogo, 0.2f);
        }
    }
}