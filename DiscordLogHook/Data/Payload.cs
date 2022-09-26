using DiscordLogHook.Utilities;
using System.Collections.Generic;

namespace DiscordLogHook.Data {
    internal class Payload {
        /**
         * <summary>the message contents (up to 2000 characters)</summary>
         */
        public string content;
        /**
         * <summary>override the default username of the webhook (up to 80 characters)</summary>
         */
        public string username;
        /**
         * <summary>override the default avatar of the webhook</summary>
         */
        public string avatar_url;
        /**
         * <summary>true if this is a TTS message</summary>
         */
        public bool? tts;
        /**
         * <summary>embedded rich content</summary>
         */
        public Embed[] embeds;
        /**
         * <summary>allowed mentions for the message</summary>
         */
        public AllowedMentions allowed_mentions;
        /**
         * <summary>JSON encoded body of non-file params</summary>
         */
        public string payload_json;
        /**
         * <summary>attachment objects with filename and description</summary>
         */
        public Attachment[] attachments;
        /**
         * <summary>message flags combined as a bitfield (only SUPPRESS_EMBEDS can be set)</summary>
         */
        public int? flags;
        /**
         * <summary>name of thread to create (requires the webhook channel to be a forum channel)</summary>
         */
        public string thread_name;

        public Payload() { }

        public Payload(string message, List<string> previousLines, int mainColor = 16744448, int historyColor = 32767) {
            if (previousLines != null && previousLines.Count > 0) {
                var historyPrefix = previousLines.Count == DiscordLogger.Settings.rollingLimit ? "...\n" : "";
                embeds = new Embed[2] {
                    new Embed() {
                        description = historyPrefix + string.Join("\n", previousLines),
                        color = historyColor
                    },
                    new Embed() {
                        description = message,
                        color = mainColor
                    }
                };
            } else {
                embeds = new Embed[1]{
                    new Embed() {
                        description = message,
                        color = mainColor
                    }
                };
            }
        }

        public Payload(string message, string trace, List<string> previousLines, int mainColor = 16711807, int traceColor = 8388863, int historyColor = 32767) {
            if (previousLines != null && previousLines.Count > 0) {
                var historyPrefix = previousLines.Count == DiscordLogger.Settings.rollingLimit ? "...\n" : "";
                embeds = new Embed[3] {
                    new Embed() {
                        description = historyPrefix + string.Join("\n", previousLines),
                        color = historyColor
                    },
                    new Embed() {
                        description = message,
                        color = mainColor
                    },
                    new Embed() {
                        description = trace,
                        color = traceColor
                    }
            };
            } else {
                embeds = new Embed[2] {
                    new Embed() {
                        description = message,
                        color = mainColor
                    },
                    new Embed() {
                        description = trace,
                        color = traceColor
                    }
                };
            }
        }

        public static Payload Info(string message) {
            return new Payload(message, null, 32767);
        }

        public static Payload Warn(string message, List<string> previousLines) {
            return new Payload(message, previousLines, 16744448);
        }

        public static Payload Err(string message, List<string> previousLines) {
            return new Payload(message, previousLines, 16711807);
        }

        public static Payload Err(string message, string trace, List<string> previousLines) {
            return new Payload(message, trace, previousLines, 16711807);
        }

        public string Serialize() {
            return Json<Payload>.Serialize(this);
        }
    }
}
