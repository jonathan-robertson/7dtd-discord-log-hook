﻿using DiscordLogHook.Data;
using DiscordLogHook.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordLogHook.Commands {
    internal class ConsoleCmdDiscordLogHook : ConsoleCmdAbstract {
        private static readonly string[] Commands = new string[] {
            "discordLogHook",
            "dlh"
        };
        private readonly string help;

        public ConsoleCmdDiscordLogHook() {
            /* Add the following commands:
             * add another webhook
             * remove a webhook
             * clear all webhooks / reset config
             * test specific webhook
             * adjust rolling inf limit
             */
            Dictionary<string, string> dict = new Dictionary<string, string>() {
                { "", "show current settings" },
                { "add <log|status> <url>", "register a webhook url to either log or status" },
                { "remove <log|status> <url>", "remove a webhook url to either log or status" },
                { "clear <log|status>", "clear all webhooks from either log or status (disabling it)" },
                { "ignore add <text>", "add an item to the ignore list to be treated as INFO rather than WARN or ERR" },
                { "ignore remove <text>", "remove an item from the ignore list" },
                { "ignore clear", "clear the ignore list" },
                { "limit <number>", "adjust the number of previous INFO log entries you include in warning/error webhook messages" },
                { "test <url>", "send a test message to the provided webhook url to ensure you have a solid connection; must press enter twice" },
            };

            int i = 1; int j = 1;
            help = $"Usage:\n  {string.Join("\n  ", dict.Keys.Select(command => $"{i++}. {GetCommands()[0]} {command}").ToList())}\nDescription Overview\n{string.Join("\n", dict.Values.Select(description => $"{j++}. {description}").ToList())}";
        }

        public override string[] GetCommands() {
            return Commands;
        }

        public override string GetDescription() {
            return "Configure or adjust settings for your Discord Log Hook.";
        }

        public override string GetHelp() {
            return help;
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo) {
            try {
                if (_params.Count == 0) {
                    SdtdConsole.Instance.Output($"= Settings =\n{Json<Settings>.Serialize(DiscordLogger.Settings)}");
                    return;
                }
                switch (_params[0].ToLower()) {
                    case "add":
                        if (_params.Count != 3) {
                            break;
                        }
                        switch (_params[1].ToLower()) {
                            case "log":
                                DiscordLogger.Settings.loggerWebhooks.Add(_params[2]);
                                SettingsManager.Save(DiscordLogger.Settings);
                                return;
                            case "status":
                                DiscordLogger.Settings.statusWebhooks.Add(_params[2]);
                                SettingsManager.Save(DiscordLogger.Settings);
                                return;
                        }
                        break;
                    case "remove":
                        if (_params.Count != 3) {
                            break;
                        }
                        switch (_params[1].ToLower()) {
                            case "log":
                                DiscordLogger.Settings.loggerWebhooks.Remove(_params[2]);
                                SettingsManager.Save(DiscordLogger.Settings);
                                return;
                            case "status":
                                DiscordLogger.Settings.statusWebhooks.Remove(_params[2]);
                                SettingsManager.Save(DiscordLogger.Settings);
                                return;
                        }
                        break;
                    case "clear":
                        if (_params.Count != 2) {
                            break;
                        }
                        switch (_params[1].ToLower()) {
                            case "log":
                                DiscordLogger.Settings.loggerWebhooks.Clear();
                                SettingsManager.Save(DiscordLogger.Settings);
                                return;
                            case "status":
                                DiscordLogger.Settings.statusWebhooks.Clear();
                                SettingsManager.Save(DiscordLogger.Settings);
                                return;
                        }
                        break;
                    case "ignore":
                        switch (_params[1].ToLower()) {
                            case "add":
                                if (_params.Count != 3) {
                                    break;
                                }
                                DiscordLogger.Settings.loggerIgnorelist.Add(_params[2]);
                                SettingsManager.Save(DiscordLogger.Settings);
                                SdtdConsole.Instance.Output("ignore entry added");
                                return;
                            case "remove":
                                if (_params.Count != 3) {
                                    break;
                                }
                                if (DiscordLogger.Settings.loggerIgnorelist.Remove(_params[2])) {
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    SdtdConsole.Instance.Output("ignore entry removed");
                                } else {
                                    SdtdConsole.Instance.Output("could not find an ignore entry matching what you provided");
                                }
                                return;
                            case "clear":
                                if (DiscordLogger.Settings.loggerIgnorelist.Count > 0) {
                                    DiscordLogger.Settings.loggerIgnorelist.Clear();
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    SdtdConsole.Instance.Output("ignore list cleared");
                                } else {
                                    SdtdConsole.Instance.Output("ignore list was already clear");
                                }
                                return;
                        }
                        break;
                    case "limit":
                        if (_params.Count != 2) {
                            break;
                        }
                        if (int.TryParse(_params[1], out var limit)) {
                            DiscordLogger.Settings.rollingLimit = limit;
                            SettingsManager.Save(DiscordLogger.Settings);
                            DiscordLogger.RollingQueue.UpdateLimit(limit);
                            return;
                        }
                        break;
                    case "test":
                        if (_params.Count != 2) {
                            break;
                        }
                        ThreadManager.StartCoroutine(DiscordLogger.Send(_params[1], Payload.Info("Test Message").Serialize()));
                        return;
                }
                SdtdConsole.Instance.Output($"Invald request; run 'help {Commands[0]}' for more info");
            } catch (Exception e) {
                SdtdConsole.Instance.Output($"Exception encountered: \"{e.Message}\"\n{e.StackTrace}");
            }
        }
    }
}