using System.Collections.Generic;
using UnityEngine;

namespace DiscordLogHook.Data
{
    public class Settings
    {
        public static readonly string DefaultMessageOnGameShutdown = "⏹️ Server stopped";
        public static readonly string DefaultMessageOnGameAwake = "♻️ Server starting, should be ready for players within a few minutes ⏱️";
        public static readonly string DefaultMessageOnGameStartDone = "✅ Server ready to receive players 🎉";

        public int RollingLimit { get; set; } = 10;
        public int LogLevel { get; set; } = (int)LogType.Warning;
        public List<string> LoggerWebhooks { get; set; } = new List<string>();
        public List<string> LoggerIgnorelist { get; set; } = new List<string>();
        public List<string> StatusWebhooks { get; set; } = new List<string>();
        public string MessageOnGameShutdown { get; set; } = "";
        public string MessageOnGameAwake { get; set; } = "";
        public string MessageOnGameStartDone { get; set; } = "";

        public LogType GetLogLevel()
        {
            return (LogType)LogLevel;
        }

        public void SetLogLevel(LogType logType)
        {
            LogLevel = (int)logType;
        }

        public string GetMessageForShutdown()
        {
            return string.IsNullOrEmpty(MessageOnGameShutdown)
                ? DefaultMessageOnGameShutdown
                : MessageOnGameShutdown;
        }

        public string GetMessageForAwake()
        {
            return string.IsNullOrEmpty(MessageOnGameAwake)
                ? DefaultMessageOnGameAwake
                : MessageOnGameAwake;
        }

        public string GetMessageForStartDone()
        {
            return string.IsNullOrEmpty(MessageOnGameStartDone)
                ? DefaultMessageOnGameStartDone
                : MessageOnGameStartDone;
        }

        public override string ToString()
        {
            var noEntries = "[no entries]";
            var loggerWebhookString = $"\n\t- {string.Join("\n\t- ", LoggerWebhooks)}";
            var statusWebhookString = $"\n\t- {string.Join("\n\t- ", StatusWebhooks)}";
            var loggerIgnoreString = $"\n\t- {string.Join("\n\t- ", LoggerIgnorelist)}";

            return $@"Status Settings
- status urls: {(StatusWebhooks.Count > 0 ? statusWebhookString : noEntries)}
- shutdown message: {(MessageOnGameShutdown.Length > 0 ? MessageOnGameShutdown : DefaultMessageOnGameShutdown)}
- awake message: {(MessageOnGameAwake.Length > 0 ? MessageOnGameAwake : DefaultMessageOnGameAwake)}
- ready message: {(MessageOnGameStartDone.Length > 0 ? MessageOnGameStartDone : DefaultMessageOnGameStartDone)}

Logger Settings
- log level: {GetLogLevel()}
- log limit: {RollingLimit}
- logger urls: {(LoggerWebhooks.Count > 0 ? loggerWebhookString : noEntries)}
- ignore list: {(LoggerIgnorelist.Count > 0 ? loggerIgnoreString : noEntries)}";
        }
    }
}
