using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

namespace MondayOFF {
    public static partial class EveryDay {
        private static Logger _unityLogger = default;
        private static ILogger _everydayLogger = new EverydayLogger();

        private static void PrepareLogger() {
#if !UNITY_EDITOR
            _unityLogger = new Logger(Debug.unityLogger);
            UseEverydayLogger();
#endif
        }

        private static void UseEverydayLogger() {
            _unityLogger.Log("[EVERYDAY] Using Everyday logger");
            AssignLogger(_everydayLogger);
        }

        private static void AssignLogger(ILogger logger) {
            Type type = typeof(Debug);
            FieldInfo fieldInfo = type.GetField("s_Logger", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            fieldInfo.SetValue(null, logger);
        }

        private class EverydayLogger : ILogger {
            public ILogHandler logHandler {
                get => _unityLogger.logHandler;
                set => _unityLogger.logHandler = value;
            }
            public bool logEnabled {
                get => _unityLogger.logEnabled;
                set => _unityLogger.logEnabled = value;
            }
            public LogType filterLogType {
                get => _unityLogger.filterLogType;
                set => _unityLogger.filterLogType = value;
            }
            public void LogFormat(LogType logType, Object context, string format, params object[] args) {
                if (settings.verboseLogging)
                    _unityLogger.LogFormat(logType, context, format, args);
            }

            public void LogException(Exception exception, Object context) {
                if (settings.verboseLogging)
                    _unityLogger.LogException(exception, context);
            }

            public bool IsLogTypeAllowed(LogType logType) {
                return settings.verboseLogging;
            }

            public void Log(LogType logType, object message) {
                if (settings.verboseLogging)
                    _unityLogger.Log(logType, message);
            }

            public void Log(LogType logType, object message, Object context) {
                if (settings.verboseLogging)
                    _unityLogger.Log(logType, message, context);
            }

            public void Log(LogType logType, string tag, object message) {
                if (settings.verboseLogging)
                    _unityLogger.Log(logType, tag, message);
            }

            public void Log(LogType logType, string tag, object message, Object context) {
                if (settings.verboseLogging)
                    _unityLogger.Log(logType, tag, message, context);
            }

            public void Log(object message) {
                if (settings.verboseLogging)
                    _unityLogger.Log(message);
            }

            public void Log(string tag, object message) {
                if (settings.verboseLogging)
                    _unityLogger.Log(tag, message);
            }

            public void Log(string tag, object message, Object context) {
                if (settings.verboseLogging)
                    _unityLogger.Log(tag, message, context);
            }

            public void LogWarning(string tag, object message) {
                if (settings.verboseLogging)
                    _unityLogger.LogWarning(tag, message);
            }

            public void LogWarning(string tag, object message, Object context) {
                if (settings.verboseLogging)
                    _unityLogger.LogWarning(tag, message, context);
            }

            public void LogError(string tag, object message) {
                if (settings.verboseLogging)
                    _unityLogger.LogWarning(tag, message);
            }

            public void LogError(string tag, object message, Object context) {
                if (settings.verboseLogging)
                    _unityLogger.LogWarning(tag, message, context);
            }

            public void LogFormat(LogType logType, string format, params object[] args) {
                if (settings.verboseLogging)
                    _unityLogger.LogFormat(logType, format, args);
            }

            public void LogException(Exception exception) {
                if (settings.verboseLogging)
                    _unityLogger.LogException(exception);
            }
        }
    }
}