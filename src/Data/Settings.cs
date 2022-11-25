using System.Collections.Generic;
using UnityEngine;

namespace DiscordLogHook.Utilities {
    internal class Settings {
        public static readonly string DefaultMessageOnGameShutdown = "⏹️ Server stopped";
        public static readonly string DefaultMessageOnGameAwake = "♻️ Server starting, should be ready for players within a few minutes ⏱️";
        public static readonly string DefaultMessageOnGameStartDone = "✅ Server ready to receive players 🎉";

        public int rollingLimit = 10;
        public int logLevel = (int)LogType.Warning;
        public List<string> loggerWebhooks = new List<string>();
        public List<string> loggerIgnorelist = new List<string>();
        public List<string> statusWebhooks = new List<string>();
        public string messageOnGameShutdown = "";
        public string messageOnGameAwake = "";
        public string messageOnGameStartDone = "";

        public LogType GetLogLevel() {
            return (LogType)logLevel;
        }

        public void SetLogLevel(LogType logType) {
            logLevel = (int)logType;
        }

        public string GetMessageForShutdown() {
            return string.IsNullOrEmpty(messageOnGameShutdown)
                ? DefaultMessageOnGameShutdown
                : messageOnGameShutdown;
        }

        public string GetMessageForAwake() {
            return string.IsNullOrEmpty(messageOnGameAwake)
                ? DefaultMessageOnGameAwake
                : messageOnGameAwake;
        }

        public string GetMessageForStartDone() {
            return string.IsNullOrEmpty(messageOnGameStartDone)
                ? DefaultMessageOnGameStartDone
                : messageOnGameStartDone;
        }

        public override string ToString() {
            string noEntries = "[no entries]";
            string loggerWebhookString = $"\n\t- {string.Join("\n\t- ", loggerWebhooks)}";
            string statusWebhookString = $"\n\t- {string.Join("\n\t- ", statusWebhooks)}";
            string loggerIgnoreString = $"\n\t- {string.Join("\n\t- ", loggerIgnorelist)}";

            return $@"Status Settings
- status urls: {(statusWebhooks.Count > 0 ? statusWebhookString : noEntries)}
- shutdown message: {(messageOnGameShutdown.Length > 0 ? messageOnGameShutdown : DefaultMessageOnGameShutdown)}
- awake message: {(messageOnGameAwake.Length > 0 ? messageOnGameAwake : DefaultMessageOnGameAwake)}
- ready message: {(messageOnGameStartDone.Length > 0 ? messageOnGameStartDone : DefaultMessageOnGameStartDone)}

Logger Settings
- log level: {GetLogLevel()}
- log limit: {rollingLimit}
- logger urls: {(loggerWebhooks.Count > 0 ? loggerWebhookString : noEntries)}
- ignore list: {(loggerIgnorelist.Count > 0 ? loggerIgnoreString : noEntries)}";
        }
    }
}
