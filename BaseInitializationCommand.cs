using Cysharp.Threading.Tasks;

namespace FishRoom.Initialization
{
    public abstract class BaseInitializationCommand : IInitializationCommand
    {
        protected BaseInitializationCommand(InitializationContext context)
        {
            Context = context;
        }

        protected InitializationContext Context { get; }

        public abstract string Name { get; }

        public abstract UniTask InitAsync();
    }
}