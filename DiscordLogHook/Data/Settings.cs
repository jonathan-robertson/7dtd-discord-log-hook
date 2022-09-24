using System.Collections.Generic;

namespace DiscordLogHook.Utilities {
    internal class Settings {
        public int rollingLimit = 10;
        public List<string> loggerWebhooks = new List<string>();
        public List<string> loggerIgnorelist = new List<string>();
        public List<string> statusWebhooks = new List<string>();
        public string messageOnGameAwake = ":recycle: Server starting...\n> *:stopwatch: typically ready for players within 1-2 minutes*";
        public string messageOnGameStartDone = ":white_check_mark: Server ready to receive players :tada:";
        public string messageOnGameShutdown = ":stop_button: Server shutting down";
    }
}
