using Cysharp.Threading.Tasks;
using Game.OfferSystem;

namespace Game.Initialization.Commands
{
    public class OfferInitializationCommand : BaseInitializationCommand
    {
        public OfferInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }
        public override UniTask InitAsync()
        {
            Context.Container.BindInterfacesAndSelfTo<OfferController>().AsSingle();

            return UniTask.CompletedTask;
        }
    }
}