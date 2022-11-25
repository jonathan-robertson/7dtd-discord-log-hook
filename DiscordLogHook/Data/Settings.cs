using System.Collections.Generic;
using UnityEngine;

namespace DiscordLogHook.Utilities {
    internal class Settings {
        public static readonly string DefaultMessageOnGameShutdown = "⏹️ Server stopped";
        public static readonly string DefaultMessageOnGameAwake = "♻️ Server starting, should be ready for players within a few minutes ⏱️";
        public static readonly string DefaultMessageOnGameStartDone = "✅ Server ready to receive players 🎉";

        public int rollingLimit = 10;
        public LogType LogLevel { get; internal set; } = LogType.Warning;
        public List<string> loggerWebhooks = new List<string>();
        public List<string> loggerIgnorelist = new List<string>();
        public List<string> statusWebhooks = new List<string>();
        public string messageOnGameShutdown = "";
        public string messageOnGameAwake = "";
        public string messageOnGameStartDone = "";

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
    }
}
