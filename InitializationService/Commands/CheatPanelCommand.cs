using Cysharp.Threading.Tasks;
using Game.Cheat;

namespace Game.Initialization.Commands
{
    public class CheatPanelCommand : BaseInitializationCommand
    {
        public override string Name => "CheatPanel";

        public CheatPanelCommand(InitializationContext context) : base(context)
        {
        }

        public override UniTask InitAsync()
        {
            //TODO check environment and off cheat panel
            
            Context.Container.Bind<CheatPanel>().AsSingle().NonLazy();
            CheatPanel cheat = Context.Container.Resolve<CheatPanel>();

            SRDebug.Instance.AddOptionContainer(cheat);

            return UniTask.CompletedTask;
        }
    }
}