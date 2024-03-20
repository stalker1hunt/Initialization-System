#if  UNITY_ANDROID && UNITY_IOS

using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

namespace FishRoom.Initialization.Commands
{
    public class FirebaseInitializationCommand : BaseInitializationCommand
    {
        private DependencyStatus m_DependencyStatus = DependencyStatus.UnavailableOther;

        public FirebaseInitializationCommand(InitializationContext context) : base(context)
        {
        }

        public override string Name { get; }

        public override async Task InitAsync()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                m_DependencyStatus = task.Result;
                if (m_DependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + m_DependencyStatus);
                }
            });

            void InitializeFirebase() => FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        }
    }
}

#endif
