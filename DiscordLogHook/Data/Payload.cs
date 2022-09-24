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
         * <summary>the components to include with the message</summary>
         */
        public Component[] components;
        /**
         * <summary>the contents of the file being sent</summary>
         */
        public FileContents[] files;
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

        /*
        public Payload(string message) {
            if (message.Length > 2000) {
                content = message.Substring(0, 2000);
            } else {
                content = message;
            }
        }
        */

        // #007fff = 32767 = blue = info
        // #ff8000 = 16744448 = orange = warning
        // #ff007f = 16711807 = pink/red = err

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

        public Payload(string message, string trace, List<string> previousLines, int mainColor = 16711807, int historyColor = 32767) {
            if (previousLines != null && previousLines.Count > 0) {
                var historyPrefix = previousLines.Count == DiscordLogger.Settings.rollingLimit ? "...\n" : "";
                embeds = new Embed[2] {
                    new Embed() {
                        description = historyPrefix + string.Join("\n", previousLines),
                        color = historyColor
                    },
                    new Embed() {
                        description = $"{message}\n> {trace}",
                        color = mainColor
                    }
            };
            } else {
                embeds = new Embed[1] {
                    new Embed() {
                        description = $"{message}\n> {trace}",
                        color = mainColor
                    }
                };
            }
        }

        /*
        public Payload(string message, List<string> previousLines, int mainColor = 16744448, int historyColor = 32767) {
            var content = previousLines
                .Select(line => new Embed() { description = line, color = historyColor })
                .ToList();
            content.Add(new Embed() { description = message, color = mainColor });
            embeds = content.ToArray();
        }

        public Payload(string message, string trace, List<string> previousLines, int mainColor = 16711807, int historyColor = 32767) {
            var content = previousLines
                .Select(line => new Embed() { description = line, color = historyColor })
                .ToList();
            content.Add(new Embed() { description = $"{message}\n> {trace}", color = mainColor });
            embeds = content.ToArray();
        }
        */

        public static Payload Info(string message) {
            return new Payload(message, null, 32767);
            // return new Payload() { content = message };
        }

        public static Payload Warn(string message, List<string> previousLines) {
            return new Payload(message, previousLines);
        }

        public static Payload Err(string message, List<string> previousLines) {
            return new Payload(message, previousLines, 16711807);
        }

        public static Payload Err(string message, string trace, List<string> previousLines) {
            return new Payload(message, trace, previousLines);
        }

        public string Serialize() {
            return Json<Payload>.Serialize(this);
        }
    }
}
