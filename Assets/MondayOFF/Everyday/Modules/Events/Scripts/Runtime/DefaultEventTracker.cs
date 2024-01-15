#if !FIREBASE_ENABLED || UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace MondayOFF
{
    public static class EventTracker
    {
        private static bool _isInitialized = false;

        public static void TryStage(int stageNum, string stageName = "Stage")
        {
            if (!_isInitialized)
            {
                EverydayLogger.Info("Event Tracker is NOT initialized!");
                return;
            }
            EverydayLogger.Info($"Default Event Tracker: Trying {stageName} {stageNum}");
        }

        public static void ClearStage(int stageNum, string stageName = "Stage")
        {
            // Send event regardless of initialization status
            switch (stageNum)
            {
                case 10:
                case 20:
                case 30:
                    SingularSDK.Event($"Stage{stageNum}");
                    break;
            }

            if (!_isInitialized)
            {
                EverydayLogger.Info("Event Tracker is NOT initialized!");
                return;
            }

            EverydayLogger.Info($"Default Event Tracker: Cleared {stageName} {stageNum}");
        }

        // Stringify prameter values
        public static void LogCustomEvent(string eventName, Dictionary<string, string> parameters = null)
        {
            if (!_isInitialized)
            {
                EverydayLogger.Info("Event Tracker is NOT initialized!");
                return;
            }

            if (parameters == null)
            {
                EverydayLogger.Info($"Default Event Tracker: {eventName} logged without any parameters");
            }
            else
            {
                string paramString = "\n";
                foreach (var item in parameters)
                {
                    paramString += $"{item.Key} : {item.Value}\n";
                }

                EverydayLogger.Info($"Default Event Tracker: {eventName} logged with parameters: {paramString}");
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            Initialize();
        }

        internal static void Initialize()
        {
            if (!EveryDay.isInitialized)
            {
                EveryDay.OnEverydayInitialized += Initialize;
                return;
            }

            if (_isInitialized)
            {
                EverydayLogger.Info("Event Tracker is already initialized!");
                return;
            }
            EverydayLogger.Info("Initialize Event Tracker");
            _isInitialized = true;
#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

#if UNITY_EDITOR
        private static void OnEditorStop()
        {
            EverydayLogger.Info("Stop Playmode Event Tracker");
            _isInitialized = false;
        }
#endif
    }
}
#endif