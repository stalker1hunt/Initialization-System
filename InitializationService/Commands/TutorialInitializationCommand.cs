﻿using Cysharp.Threading.Tasks;
using Game.TutorialSystem;

namespace Game.Initialization.Commands
{
    public class TutorialInitializationCommand : BaseInitializationCommand
    {
        public TutorialInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }
        
        public override UniTask InitAsync()
        {
            Context.Container.BindInterfacesAndSelfTo<TutorialController>().AsSingle();

           return UniTask.CompletedTask;
        }
    }
}