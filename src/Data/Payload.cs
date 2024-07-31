using DiscordLogHook.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscordLogHook.Data
{
    internal class Payload
    {
        /// <summary>
        /// Number of characters Discord limits content fields to.
        /// </summary>
        /// <see cref="https://discord.com/developers/docs/resources/webhook#execute-webhook-jsonform-params"/>
        private const int DISCORD_CONTENT_CHAR_LIMIT = 2000;

        private const int INFO_COLOR = 32767;
        private const int WARN_COLOR = 16744448;
        private const int ERR_COLOR = 16711807;
        private const int TRACE_COLOR = 8388863;

        private readonly string _logPattern = @"^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2} \d*\.\d{3}) (INF|WRN|ERR|EXC) (.*)";

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

        public Payload(string message)
        {
            SetContent(message);
        }

        // TODO: come to think of it... hand-jamming JSON would be more efficient and faster than what I'm doing here
        public Payload(string message, List<string> previousLines, int color = INFO_COLOR, int historyColor = INFO_COLOR)
        {
            SetContent(message);
            embeds = previousLines != null && previousLines.Count > 0
                ? (new Embed[] {
                    new Embed()
                    {
                        color = historyColor,
                        description = $"...\n{string.Join("\n", previousLines)}",
                        footer = new EmbedFooter() {
                            text = "logs leading up to event"
                        },
                    },
                    new Embed()
                    {
                        color = color,
                        description = message,
                        timestamp = DateTime.Now.ToLocalTime().ToString("s"),
                        footer = new EmbedFooter() {
                            text = "server local time"
                        },
                    },
                })
                : (new Embed[] {
                    new Embed()
                    {
                        color = color,
                        description = message,
                        timestamp = DateTime.Now.ToLocalTime().ToString("s"),
                        footer = new EmbedFooter() {
                            text = "server local time"
                        },
                    },
                });
        }

        public Payload(string message, string trace, List<string> previousLines, int color = INFO_COLOR, int historyColor = INFO_COLOR, int traceColor = TRACE_COLOR)
        {
            SetContent(message);
            embeds = previousLines != null && previousLines.Count > 0
                ? (new Embed[] {
                    new Embed()
                    {
                        color = historyColor,
                        description = $"...\n{string.Join("\n", previousLines)}",
                        footer = new EmbedFooter() {
                            text = "logs leading up to event"
                        },
                    },
                    new Embed()
                    {
                        color = color,
                        description = message,
                        timestamp = DateTime.Now.ToLocalTime().ToString("s"),
                        footer = new EmbedFooter() {
                            text = "server local time"
                        },
                    },
                    new Embed() {
                        description = trace,
                        color = traceColor,
                    },
                })
                : (new Embed[] {
                    new Embed()
                    {
                        color = color,
                        description = message,
                        timestamp = DateTime.Now.ToLocalTime().ToString("s"),
                        footer = new EmbedFooter() {
                            text = "server local time"
                        },
                    },
                    new Embed() {
                        description = trace,
                        color = traceColor,
                    },
                });
        }

        public static Payload Content(string message)
        {
            return new Payload(message);
        }

        public static Payload Info(string message, List<string> previousLines)
        {
            return new Payload(message, previousLines, INFO_COLOR);
        }

        public static Payload Warn(string message, List<string> previousLines)
        {
            return new Payload(message, previousLines, WARN_COLOR);
        }

        public static Payload Err(string message, List<string> previousLines)
        {
            return new Payload(message, previousLines, ERR_COLOR);
        }

        public static Payload Err(string message, string trace, List<string> previousLines)
        {
            return new Payload(message, trace, previousLines, ERR_COLOR);
        }

        public string Serialize()
        {
            return Json<Payload>.Serialize(this);
        }

        /// <summary>
        /// Reorganize timestamp and replace INF/WRN/ERR/EXC with an appropriate emoji and truncate message length to what Discord allows.
        /// </summary>
        /// <param name="message">Incoming message to manipulate and store.</param>
        protected void SetContent(string message)
        {
            var matches = Regex.Matches(message, _logPattern, RegexOptions.Multiline);
            if (matches.Count == 1 && matches[0].Groups.Count == 4)
            {
                switch (matches[0].Groups[2].Value)
                {
                    case "INF":
                        content = $"ℹ️ {matches[0].Groups[3].Value}\n> {matches[0].Groups[1].Value}";
                        break;
                    case "WRN":
                        content = $"⚠️ {matches[0].Groups[3].Value}\n> {matches[0].Groups[1].Value}";
                        break;
                    case "ERR":
                        content = $"📟 {matches[0].Groups[3].Value}\n> {matches[0].Groups[1].Value}";
                        break;
                    case "EXC":
                        content = $"🚨 {matches[0].Groups[3].Value}\n> {matches[0].Groups[1].Value}";
                        break;
                }
            }
            else
            {
                content = message;
            }
            if (content.Length > DISCORD_CONTENT_CHAR_LIMIT)
            {
                content = $"{content.Substring(content.Length - DISCORD_CONTENT_CHAR_LIMIT - 3)}...";
            }
        }
    }
}
