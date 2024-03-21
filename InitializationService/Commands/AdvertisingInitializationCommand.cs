using Cysharp.Threading.Tasks;
using Game.AdvertisingSystem;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Game.Initialization.Commands
{
    public class AdvertisingInitializationCommand : BaseInitializationCommand
    {
        public AdvertisingInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }

        public override UniTask InitAsync()
        {
            Context.Container.BindInterfacesAndSelfTo<AdvertisingController>().AsSingle();

            var requestConfiguration =
                new RequestConfiguration.Builder()
                    .SetSameAppKeyEnabled(true).build();

            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(HandleInitCompleteAction);
            return UniTask.CompletedTask;
        }

        private void HandleInitCompleteAction(InitializationStatus status)
        {
            Debug.Log("Init ADV completed");
        }
    }
}