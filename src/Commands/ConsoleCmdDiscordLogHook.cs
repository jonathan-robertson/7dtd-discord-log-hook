using DiscordLogHook.Data;
using DiscordLogHook.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiscordLogHook.Commands
{
    internal class ConsoleCmdDiscordLogHook : ConsoleCmdAbstract
    {
        private static readonly string[] Commands = new string[] {
            "discordLogHook",
            "dlh"
        };
        private readonly string help;

        public ConsoleCmdDiscordLogHook()
        {
            /* Add the following commands:
             * add another webhook
             * remove a webhook
             * clear all webhooks / reset config
             * test specific webhook
             * adjust rolling inf limit
             */
            var dict = new Dictionary<string, string>() {
                { "settings", "show current settings" },
                { "add <log|status> <url>", "register a webhook url to either log or status" },
                { "remove <log|status> <url>", "remove a webhook url to either log or status" },
                { "clear <log|status>", "clear all webhooks from either log or status (disabling it)" },
                { "ignore add <text>", "add an item to the ignore list to be treated as INFO rather than WARN or ERR" },
                { "ignore remove <text>", "remove an item from the ignore list" },
                { "ignore clear", "clear the ignore list" },
                { "set level <WARN|ERR>", "set the log level limit you want to include messages for. For example, choosing WARN will include both WARN and ERR messages" },
                { "set limit <number>", "adjust the size of the rolling log entry cache for previous higher-level log entries you include in alerts. In other words: when an alert fires, how many of the message before that alerting message do you want to have included?" },
                { "set message <awake|start|shutdown> <message>", $"set the given server status message; set as \"\" to use default messages...\n  DefaultMessageOnGameShutdown: {Settings.DefaultMessageOnGameShutdown}\n  DefaultMessageOnGameAwake: {Settings.DefaultMessageOnGameAwake}\n  DefaultMessageOnGameStartDone: {Settings.DefaultMessageOnGameStartDone}" },
                { "test <url>", "send a test message to the provided webhook url to ensure you have a solid connection; must press enter twice" },
            };

            var i = 1; var j = 1;
            help = $"Usage:\n  {string.Join("\n  ", dict.Keys.Select(command => $"{i++}. {GetCommands()[0]} {command}").ToList())}\nDescription Overview\n{string.Join("\n", dict.Values.Select(description => $"{j++}. {description}").ToList())}";
        }

        protected override string[] getCommands()
        {
            return Commands;
        }

        protected override string getDescription()
        {
            return "Configure or adjust settings for your Discord Log Hook.";
        }

        public override string GetHelp()
        {
            return help;
        }

        public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
        {
            try
            {
                if (_params.Count > 0)
                {
                    switch (_params[0].ToLower())
                    {
                        case "settings":
                            SdtdConsole.Instance.Output(DiscordLogger.Settings.ToString());
                            return;
                        case "add":
                            if (_params.Count != 3)
                            {
                                break;
                            }
                            switch (_params[1].ToLower())
                            {
                                case "log":
                                    DiscordLogger.Settings.LoggerWebhooks.Add(_params[2]);
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    return;
                                case "status":
                                    DiscordLogger.Settings.StatusWebhooks.Add(_params[2]);
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    return;
                            }
                            break;
                        case "remove":
                            if (_params.Count != 3)
                            {
                                break;
                            }
                            switch (_params[1].ToLower())
                            {
                                case "log":
                                    _ = DiscordLogger.Settings.LoggerWebhooks.Remove(_params[2]);
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    return;
                                case "status":
                                    _ = DiscordLogger.Settings.StatusWebhooks.Remove(_params[2]);
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    return;
                            }
                            break;
                        case "clear":
                            if (_params.Count != 2)
                            {
                                break;
                            }
                            switch (_params[1].ToLower())
                            {
                                case "log":
                                    DiscordLogger.Settings.LoggerWebhooks.Clear();
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    return;
                                case "status":
                                    DiscordLogger.Settings.StatusWebhooks.Clear();
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    return;
                            }
                            break;
                        case "ignore":
                            switch (_params[1].ToLower())
                            {
                                case "add":
                                    if (_params.Count != 3)
                                    {
                                        break;
                                    }
                                    DiscordLogger.Settings.LoggerIgnorelist.Add(_params[2]);
                                    SettingsManager.Save(DiscordLogger.Settings);
                                    SdtdConsole.Instance.Output("ignore entry added");
                                    return;
                                case "remove":
                                    if (_params.Count != 3)
                                    {
                                        break;
                                    }
                                    if (DiscordLogger.Settings.LoggerIgnorelist.Remove(_params[2]))
                                    {
                                        SettingsManager.Save(DiscordLogger.Settings);
                                        SdtdConsole.Instance.Output("ignore entry removed");
                                    }
                                    else
                                    {
                                        SdtdConsole.Instance.Output("could not find an ignore entry matching what you provided");
                                    }
                                    return;
                                case "clear":
                                    if (DiscordLogger.Settings.LoggerIgnorelist.Count > 0)
                                    {
                                        DiscordLogger.Settings.LoggerIgnorelist.Clear();
                                        SettingsManager.Save(DiscordLogger.Settings);
                                        SdtdConsole.Instance.Output("ignore list cleared");
                                    }
                                    else
                                    {
                                        SdtdConsole.Instance.Output("ignore list was already clear");
                                    }
                                    return;
                            }
                            break;
                        case "set":
                            switch (_params[1].ToLower())
                            {
                                case "level":
                                    if (_params.Count != 3) { break; }
                                    switch (_params[2].ToLower())
                                    {
                                        case "warn":
                                            DiscordLogger.Settings.SetLogLevel(LogType.Warning);
                                            SettingsManager.Save(DiscordLogger.Settings);
                                            SdtdConsole.Instance.Output($"successfully updated log level to {LogType.Warning}");
                                            return;
                                        case "err":
                                            DiscordLogger.Settings.SetLogLevel(LogType.Error);
                                            SettingsManager.Save(DiscordLogger.Settings);
                                            SdtdConsole.Instance.Output($"successfully updated log level to {LogType.Error}");
                                            return;
                                    }
                                    break;
                                case "limit":
                                    if (_params.Count != 3)
                                    {
                                        break;
                                    }
                                    if (int.TryParse(_params[2], out var limit))
                                    {
                                        DiscordLogger.Settings.RollingLimit = limit;
                                        SettingsManager.Save(DiscordLogger.Settings);
                                        DiscordLogger.RollingQueue.UpdateLimit(limit);
                                        SdtdConsole.Instance.Output($"successfully updated log limit to {limit}");
                                        return;
                                    }
                                    break;
                                case "message":
                                    if (_params.Count != 4) { break; }
                                    switch (_params[2].ToLower())
                                    {
                                        case "awake":
                                            DiscordLogger.Settings.MessageOnGameAwake = _params[3];
                                            SettingsManager.Save(DiscordLogger.Settings);
                                            SdtdConsole.Instance.Output($"successfully updated MessageOnGameAwake to {_params[3]}");
                                            return;
                                        case "start":
                                            DiscordLogger.Settings.MessageOnGameStartDone = _params[3];
                                            SettingsManager.Save(DiscordLogger.Settings);
                                            SdtdConsole.Instance.Output($"successfully updated MessageOnGameStartDone to {_params[3]}");
                                            return;
                                        case "shutdown":
                                            DiscordLogger.Settings.MessageOnGameShutdown = _params[3];
                                            SettingsManager.Save(DiscordLogger.Settings);
                                            SdtdConsole.Instance.Output($"successfully updated MessageOnGameShutdown to {_params[3]}");
                                            return;
                                    }
                                    break;
                            }
                            break;
                        case "test":
                            if (_params.Count != 2)
                            {
                                break;
                            }
                            _ = ThreadManager.StartCoroutine(DiscordLogger.Send(_params[1], Payload.Info("Test Message").Serialize()));
                            return;
                    }
                }
                SdtdConsole.Instance.Output($"Invald request; run 'help {Commands[0]}' for more info");
            }
            catch (Exception e)
            {
                SdtdConsole.Instance.Output($"Exception encountered: \"{e.Message}\"\n{e.StackTrace}");
            }
        }
    }
}
