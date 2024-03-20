using Cysharp.Threading.Tasks;

namespace FishRoom.Initialization
{
    public interface IInitializationCommand
    {
        string Name { get; }

        UniTask InitAsync();
    }
}