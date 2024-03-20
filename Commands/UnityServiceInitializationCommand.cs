using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace FishRoom.Initialization.Commands
{
    public class UnityServiceInitializationCommand : BaseInitializationCommand
    {
        public UnityServiceInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }

        public override async UniTask InitAsync()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return;

            var options = new InitializationOptions();

            options.SetEnvironmentName("dev");
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}