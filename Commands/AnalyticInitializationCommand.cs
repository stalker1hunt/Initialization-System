using Cysharp.Threading.Tasks;
using FishRoom.AnalyticSystem;

namespace FishRoom.Initialization.Commands
{
    public class AnalyticInitializationCommand: BaseInitializationCommand
    {
        public AnalyticInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }
        public override UniTask InitAsync()
        {
            IAnalyticService analyticService = new AnalyticService();
            Context.Container.BindInterfacesAndSelfTo<AnalyticController>().AsSingle().WithArguments(analyticService);

            var analyticController = Context.Container.Resolve<IAnalyticController>();
            analyticController.Init();

            return UniTask.CompletedTask;
        }
    }
}