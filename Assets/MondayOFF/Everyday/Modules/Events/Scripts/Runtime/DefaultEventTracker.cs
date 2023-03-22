#if !FIREBASE_ENABLED || UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace MondayOFF {
    public static class EventTracker {
        private static bool _isInitialized = false;

        public static void TryStage(int stageNum, string stageName = "Stage") {
            if (!_isInitialized) {
                Debug.Log("[EVERYDAY] Event Tracker is NOT initialized!");
                return;
            }
            Debug.Log($"[EVERYDAY] Default Event Tracker: Trying {stageName} {stageNum}");
        }

        public static void ClearStage(int stageNum, string stageName = "Stage") {
            // Send event regardless of initialization status
            switch (stageNum) {
                case 10:
                case 20:
                case 30:
                    SingularSDK.Event($"Stage{stageNum}");
                    break;
            }

            if (!_isInitialized) {
                Debug.Log("[EVERYDAY] Event Tracker is NOT initialized!");
                return;
            }

            Debug.Log($"[EVERYDAY] Default Event Tracker: Cleared {stageName} {stageNum}");
        }

        // Stringify prameter values
        public static void LogCustomEvent(string eventName, Dictionary<string, string> parameters = null) {
            if (!_isInitialized) {
                Debug.Log("[EVERYDAY] Event Tracker is NOT initialized!");
                return;
            }

            if (parameters == null) {
                Debug.Log($"[EVERYDAY] Default Event Tracker: {eventName} logged without any parameters");
            } else {
                string paramString = "\n";
                foreach (var item in parameters) {
                    paramString += $"{item.Key} : {item.Value}\n";
                }

                Debug.Log($"[EVERYDAY] Default Event Tracker: {eventName} logged with parameters: {paramString}");
            }
        }

        internal static void Initialize() {
            if (_isInitialized) {
                Debug.Log("[EVERYDAY] Event Tracker is already initialized!");
                return;
            }
            Debug.Log("[EVERYDAY] Initialize Event Tracker");
            _isInitialized = true;
#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

#if UNITY_EDITOR
        private static void OnEditorStop() {
            Debug.Log("[EVERYDAY] Stop Playmode Event Tracker");
            _isInitialized = false;
        }
#endif
    }
}
#endif