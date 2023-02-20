#if FIREBASE_ENABLED && !UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

namespace MondayOFF {
    public static class EventTracker {
#if UNITY_ANDROID
        private static Firebase.FirebaseApp _app = null;
#endif
        private static bool _isInitialized = true;

        public static void TryStage(int stageNum, string stageName = "Stage") {
            if (!_isInitialized) { return; }

            FirebaseAnalytics.LogEvent("Try",
                new Parameter(stageName, $"{stageName} {stageNum:000}")
            );
        }

        public static void ClearStage(int stageNum, string stageName = "Stage") {
            // Send event regardless of initialization
            switch (stageNum) {
                case 10:
                case 20:
                case 30:
                    SingularSDK.Event($"Stage_{stageNum}");
                    break;
            }

            if (!_isInitialized) { return; }

            FirebaseAnalytics.LogEvent("Clear",
                new Parameter(stageName, $"{stageName} {stageNum:000}")
            );
        }

        // Stringify prameter values
        public static void LogCustomEvent(string eventName, Dictionary<string, string> parameters) {
            if (!_isInitialized) { return; }

            if (parameters == null) {
                FirebaseAnalytics.LogEvent(eventName);
            } else {
                var eventParams = new Parameter[parameters.Count];
                int i = 0;
                foreach (var item in parameters) {
                    eventParams[i++] = new Parameter(item.Key, item.Value);
                }
                FirebaseAnalytics.LogEvent(eventName, eventParams);
            }
        }

        internal static void Initialize() {
#if UNITY_ANDROID
            _isInitialized = false;
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available) {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    _app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    OnFirebaseInitialized();

                } else {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is NOT safe to use here.
                }
            });
#else
            OnFirebaseInitialized();
#endif
        }

        private static void OnFirebaseInitialized() {
            _isInitialized = true;

            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
        }
    }
}
#endif