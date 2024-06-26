using Cysharp.Threading.Tasks;
using Game.Controllers;
using Game.Initialization.Game;
using Game.Util;

namespace Game.Initialization.Commands
{
    public class GameInitializationCommand : BaseInitializationCommand
    {
        public GameInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name => GlobalConstants.GameInizialization;

        public override UniTask InitAsync()
        {
            Context.Container.BindInterfacesAndSelfTo<FishController>().AsSingle();
            Context.Container.BindInterfacesAndSelfTo<VegetationController>().AsSingle();
            Context.Container.BindInterfacesAndSelfTo<LocationController>().AsSingle();
            Context.Container.BindInterfacesAndSelfTo<PopupController>().AsSingle();
            Context.Container.BindInterfacesAndSelfTo<FishFactoryData>().AsSingle();
            Context.Container.BindInterfacesAndSelfTo<ApplicationQuitController>().AsSingle().NonLazy();
            Context.Container.BindInterfacesAndSelfTo<IdleController>().AsSingle().WithArguments(new AsyncTimer());

            var idleController = Context.Container.Resolve<IIdleController>();
            idleController.Initialize();

            return UniTask.CompletedTask;
        }
    }
}