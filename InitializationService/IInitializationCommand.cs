using Cysharp.Threading.Tasks;

namespace Game.Initialization
{
    public interface IInitializationCommand
    {
        string Name { get; }

        UniTask InitAsync();
    }
}