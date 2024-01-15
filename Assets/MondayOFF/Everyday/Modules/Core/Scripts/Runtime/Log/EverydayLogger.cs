namespace MondayOFF {
    public static class EverydayLogger {
        private const string EVERYDAY_TAG = "[EVERYDAY]";
#if UNITY_EDITOR
        private const string ERROR_TAG = "[<color=red>ERROR</color>]";
        private const string WARN_TAG = "[<color=yellow>WARN</color>]";
        private const string INFO_TAG = "[<color=blue>INFO</color>]";
        private const string DEBUG_TAG = "[<color=green>DEBUG</color>]";
#else
        private const string ERROR_TAG = "[ERROR]";
        private const string WARN_TAG = "[WARN]";
        private const string INFO_TAG = "[INFO]";
        private const string DEBUG_TAG = "[DEBUG]";
#endif

#if EVERYDAY_NO_LOG
        [System.Diagnostics.Conditional("EVERYDAY_DONT_SHOW_LOGGING_NAME_SOMETHING_NEVER_USED")]
#endif
        public static void Error(string message) {
            if (EverydaySettings.Instance.logLevel >= LogLevel.Error)
                UnityEngine.Debug.LogError($"{EVERYDAY_TAG}{ERROR_TAG} {message}");
        }

#if EVERYDAY_NO_LOG
        [System.Diagnostics.Conditional("EVERYDAY_DONT_SHOW_LOGGING_NAME_SOMETHING_NEVER_USED")]
#endif
        public static void Error(object message) {
            if (EverydaySettings.Instance.logLevel >= LogLevel.Error)
                UnityEngine.Debug.LogError($"{EVERYDAY_TAG}{ERROR_TAG} {message}");
        }

#if EVERYDAY_NO_LOG
        [System.Diagnostics.Conditional("EVERYDAY_DONT_SHOW_LOGGING_NAME_SOMETHING_NEVER_USED")]
#endif
        public static void Warn(string message) {
            if (EverydaySettings.Instance.logLevel >= LogLevel.Warning)
                UnityEngine.Debug.LogWarning($"{EVERYDAY_TAG}{WARN_TAG} {message}");

        }

#if EVERYDAY_NO_LOG
        [System.Diagnostics.Conditional("EVERYDAY_DONT_SHOW_LOGGING_NAME_SOMETHING_NEVER_USED")]
#endif
        public static void Info(string message) {
            if (EverydaySettings.Instance.logLevel >= LogLevel.Info)
                UnityEngine.Debug.Log($"{EVERYDAY_TAG}{INFO_TAG} {message}");
        }


#if EVERYDAY_NO_LOG
        [System.Diagnostics.Conditional("EVERYDAY_DONT_SHOW_LOGGING_NAME_SOMETHING_NEVER_USED")]
#endif
        public static void Debug(string message) {
            if (EverydaySettings.Instance.logLevel >= LogLevel.Debug)
                UnityEngine.Debug.Log($"{EVERYDAY_TAG}{INFO_TAG} {message}");
        }
    }
}