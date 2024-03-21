using CrazyGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Initialization.Commands
{
    public class CrazySDKInitializationCommand : BaseInitializationCommand
    {
        public CrazySDKInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }
        public override UniTask InitAsync()
        {
            if (CrazySDK.Instance.IsInitialized)
            {
                Debug.Log("Init CrazySDK completed");
            }

            return UniTask.CompletedTask;
        }
    }
}