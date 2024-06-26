﻿using Cysharp.Threading.Tasks;
using Game.Controllers;

namespace Game.Initialization.Commands
{
    public class EconomyInitializationCommand : BaseInitializationCommand
    {
        public EconomyInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name => GlobalConstants.Economy;

        public override UniTask InitAsync()
        {
            Context.Container.BindInterfacesAndSelfTo<EconomyController>().AsSingle();
            return UniTask.CompletedTask;
        }
    }
}