using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Game.Initialization
{
    public class InitializationBootstrapper
    {
        private readonly List<IInitializationCommand> _initializationCommands = new List<IInitializationCommand>();
        private readonly List<Type> _initializationCommandTypes = new List<Type>();
        private readonly InitializationContext _initializationContext;
        private readonly List<Type> _preInitializationCommandTypes = new List<Type>();

        public InitializationBootstrapper(InitializationContext initializationContext)
        {
            _initializationContext = initializationContext;
        }

        public float Progress { get; private set; }

        public void AddPreInitializationCommand<T>() where T : BaseInitializationCommand
        {
            _preInitializationCommandTypes.Add(typeof(T));
        }

        public void AddInitializationCommand<T>() where T : BaseInitializationCommand
        {
            _initializationCommandTypes.Add(typeof(T));
        }

        public async UniTask InitAsync()
        {
            await ExecutePreCommandsAsync();

            var fullStopwatch = Stopwatch.StartNew();
            var commandStopwatch = new Stopwatch();

            var targetCommandsCount = _initializationCommandTypes.Count;
            var finishedCommandsCount = 0;

            // Pre create all types, to call constructors
            foreach (var initializationCommandType in _initializationCommandTypes)
            {
                var command =
                    (IInitializationCommand) _initializationContext.Container.Instantiate(initializationCommandType,
                        new object[] {_initializationContext});

                _initializationCommands.Add(command);
            }

            Debug.Log("Started game initialization");

            foreach (var command in _initializationCommands)
            {
                commandStopwatch.Restart();
                Debug.Log($"Started initialization of '{command.Name}'");

                try
                {
                    await command.InitAsync();
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        $"Failed initialization of '{command.Name}' in {commandStopwatch.ElapsedMilliseconds} ms. Error: '{e}'");
                    continue;
                }

                finishedCommandsCount++;
                Progress = finishedCommandsCount * 1f / targetCommandsCount;
                Debug.Log($"Finished initialization of '{command.Name}' in {commandStopwatch.ElapsedMilliseconds} ms");
            }

            Debug.Log($"Finish game initialization in {fullStopwatch.ElapsedMilliseconds} ms");
        }

        private async UniTask ExecutePreCommandsAsync()
        {
            foreach (var preInitializationCommandType in _preInitializationCommandTypes)
            {
                var command =
                    (IInitializationCommand) _initializationContext.Container.Instantiate(preInitializationCommandType,
                        new object[] {_initializationContext});

                try
                {
                    await command.InitAsync();
                    Debug.Log($"Finished Initialization of '{command.Name}'");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed initialization of '{command.Name}' pre command. Error: '{e}'");
                }
            }
        }
    }
}