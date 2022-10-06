using System.Collections.Generic;

namespace DiscordLogHook.Utilities {
    internal class Settings {
        public int rollingLimit = 10;
        public List<string> loggerWebhooks = new List<string>();
        public List<string> loggerIgnorelist = new List<string>();
        public List<string> statusWebhooks = new List<string>();
        public string messageOnGameShutdown = "⏹️ Server stopped";
        public string messageOnGameAwake = "♻️ Server starting, should be ready for players within a few minutes ⏱️";
        public string messageOnGameStartDone = "✅ Server ready to receive players 🎉";
    }
}
