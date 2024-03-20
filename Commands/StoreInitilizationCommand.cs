using Cysharp.Threading.Tasks;
using FishRoom.StoreSystem;

namespace FishRoom.Initialization.Commands
{
    public class StoreInitializationCommand : BaseInitializationCommand
    {
        public StoreInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }

        public override UniTask InitAsync()
        {
            Context.Container.Bind<PurchaseController>().AsSingle();
            var storeController = Context.Container.Resolve<PurchaseController>();
            storeController.InitializePurchasing();

            return UniTask.CompletedTask;
        }
    }
}