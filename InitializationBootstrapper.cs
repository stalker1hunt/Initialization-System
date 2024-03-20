using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace FishRoom.Initialization
{
    public class InitializationBootstrapper
    {
        private readonly List<IInitializationCommand> m_InitializationCommands = new List<IInitializationCommand>();
        private readonly List<Type> m_InitializationCommandTypes = new List<Type>();
        private readonly InitializationContext m_InitializationContext;
        private readonly List<Type> m_PreInitializationCommandTypes = new List<Type>();

        public InitializationBootstrapper(InitializationContext initializationContext)
        {
            m_InitializationContext = initializationContext;
        }

        public float Progress { get; private set; }

        public void AddPreInitializationCommand<T>() where T : BaseInitializationCommand
        {
            m_PreInitializationCommandTypes.Add(typeof(T));
        }

        public void AddInitializationCommand<T>() where T : BaseInitializationCommand
        {
            m_InitializationCommandTypes.Add(typeof(T));
        }

        public async UniTask InitAsync()
        {
            await ExecutePreCommandsAsync();

            var fullStopwatch = Stopwatch.StartNew();
            var commandStopwatch = new Stopwatch();

            var targetCommandsCount = m_InitializationCommandTypes.Count;
            var finishedCommandsCount = 0;

            // Pre create all types, to call constructors
            foreach (var initializationCommandType in m_InitializationCommandTypes)
            {
                var command =
                    (IInitializationCommand) m_InitializationContext.Container.Instantiate(initializationCommandType,
                        new object[] {m_InitializationContext});

                m_InitializationCommands.Add(command);
            }

            Debug.Log("Started game initialization");

            foreach (var command in m_InitializationCommands)
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
            foreach (var preInitializationCommandType in m_PreInitializationCommandTypes)
            {
                var command =
                    (IInitializationCommand) m_InitializationContext.Container.Instantiate(preInitializationCommandType,
                        new object[] {m_InitializationContext});

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