using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public static class BsLogger
    {
        public enum LogLevel { Debug, Info, Warning, Error, Critical }

        public static LogLevel MinLogLevel { get; set; } = LogLevel.Debug;

        public static void Log(string message, Color? color = null, LogLevel level = LogLevel.Info,  Object context = null)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD

            if (level < MinLogLevel)
            {
                return;
            }

            Color logColor = color ?? GetDefaultColorForLevel(level);
            string hexColor = ColorUtility.ToHtmlStringRGB(logColor);

            string formattedMessage = $"<color=#{hexColor}>[{level.ToString().ToUpper()}] {message}</color>";

            if (level == LogLevel.Critical)
            {
                formattedMessage += " - IMMEDIATE ATTENTION NEEDED!";
            }

            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(formattedMessage, context);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Debug.LogError(formattedMessage, context);
                    break;
                case LogLevel.Debug:
                case LogLevel.Info:
                default:
                    Debug.Log(formattedMessage, context);
                    break;
            }
            #endif
        }

        private static Color GetDefaultColorForLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return Color.gray;
                case LogLevel.Warning:
                    return Color.yellow;
                case LogLevel.Error:
                    return Color.red;
                case LogLevel.Critical:
                    return new Color(1.0f, 0.3f, 0.3f, 1.0f); // 좀 더 진한 빨강
                case LogLevel.Info:
                default:
                    return Color.white;
            }
        }

        public static void LogError(string message, Object context = null) => Log(message, null, LogLevel.Error, context);
        public static void LogWarning(string message, Object context = null) => Log(message, null, LogLevel.Warning, context);
        public static void LogInfo(string message, Object context = null) => Log(message, null, LogLevel.Info, context);
        public static void LogDebug(string message, Object context = null) => Log(message, null, LogLevel.Debug, context);
        public static void LogCritical(string message, Object context = null) => Log(message, null, LogLevel.Critical, context);
    }
}