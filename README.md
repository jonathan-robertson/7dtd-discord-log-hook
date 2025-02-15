# Discord Log Hook

[![🧪 Tested On 7DTD 1.3 (b9)](https://img.shields.io/badge/🧪%20Tested%20On-7DTD%201.3%20(b9)-blue.svg)](https://7daystodie.com/)
[![📦 Automated Release](https://github.com/jonathan-robertson/7dtd-discord-log-hook/actions/workflows/release.yml/badge.svg)](https://github.com/jonathan-robertson/7dtd-discord-log-hook/actions/workflows/release.yml)

## Summary

Configurable log hook into Discord for game service monitoring.

### Support

🗪 If you would like support for this mod, please feel free to reach out to me via [Discord](https://discord.gg/hYa2sNHXya) (my username is `kanaverum`).

## Features

Hook your 7 Days to Die game server into your Discord server to receive errors and warnings (warnings optional) as they happen... along with the 10 most recent log lines of any type for context (limit configurable).

## Commands

Each of options would be called with the command `discordLogHook` or `dlh`:

- `settings`: show current settings
- `add <log|status> <url>`: register a webhook url to either log or status
- `remove <log|status> <url>`: remove a webhook url to either log or status
- `clear <log|status>`: clear all webhooks from either log or status (disabling it)
- `ignore add <text>`: add an item to the ignore list to be treated as INFO rather than WARN or ERR
- `ignore remove <text>`: remove an item from the ignore list
- `ignore clear`: clear the ignore list
- `set level <WARN|ERR>`: set the log level limit you want to include messages for. For example, choosing WARN will include both WARN and ERR messages
- `set limit <number>`: adjust the size of the rolling log entry cache for previous higher-level log entries you include in alerts. In other words: when an alert fires, how many of the message before that alerting message do you want to have included?
- `set message <awake|start|shutdown> <message>`: set the given server status message; set as \"\" to use default messages...
  - DefaultMessageOnGameShutdown: "⏹️ Server stopped"
  - DefaultMessageOnGameAwake: "♻️ Server starting, should be ready for players within a few minutes ⏱️"
  - DefaultMessageOnGameStartDone: "✅ Server ready to receive players 🎉"
- `test <url>`: send a test message to the provided webhook url to ensure you have a solid connection; must press enter twice
- `debug|dm`: enable debug mode for troubleshooting potential issues with discord requests

## Setup

Without proper installation, this mod will not work as expected. Using this guide should help to complete the installation properly.

If you have trouble getting things working, you can reach out to me for support via [Support](#support).

### Environment / EAC / Hosting Requirements

| Environment          | Compatible | Does EAC Need to be Disabled? | Who needs to install? |
| -------------------- | ---------- | ----------------------------- | --------------------- |
| Dedicated Server     | Yes        | no                            | only server           |
| Peer-to-Peer Hosting | no         | N/A                           | N/A                   |
| Single Player Game   | no         | N/A                           | N/A                   |

> 🤔 If you aren't sure what some of this means, details steps are provided below to walk you through the setup process.

### Map Considerations for Installation or Uninstallation

- Does **adding** this mod require a fresh map?
  - No! You can drop this mod into an ongoing map without any trouble.
- Does **removing** this mod require a fresh map?
  - No! You can remove this mod at any time.

### Windows/Linux Installation (Server via FTP from Windows PC)

1. 📦 Download the latest release by navigating to [this link](https://github.com/jonathan-robertson/7dtd-discord-log-hook/releases/latest/) and clicking the link for `7dtd-discord-log-hook.zip`
2. 📂 Unzip this file to a folder named `7dtd-discord-log-hook` by right-clicking it and choosing the `Extract All...` option (you will find Windows suggests extracting to a new folder named `7dtd-discord-log-hook` - this is the option you want to use)
3. 🕵️ Locate and create your mods folder (if missing):
    - Windows PC or Server: in another window, paste this address into to the address bar: `%APPDATA%\7DaysToDie`, then enter your `Mods` folder by double-clicking it. If no `Mods` folder is present, you will first need to create it, then enter your `Mods` folder after that
    - FTP: in another window, connect to your server via FTP and navigate to the game folder that should contain your `Mods` folder (if no `Mods` folder is present, you will need to create it in the appropriate location), then enter your `Mods` folder. If you are confused about where your mods folder should go, reach out to your host.
4. 🚚 Move this new `7dtd-discord-log-hook` folder into your `Mods` folder by dragging & dropping or cutting/copying & pasting, whichever you prefer
5. ♻️ Restart your server to allow this mod to take effect and monitor your logs to ensure it starts successfully:
    - you can search the logs for the word `DiscordLogHook`; the name of this mod will appear with that phrase and all log lines it produces will be presented with this prefix for quick reference

### Linux Server Installation (Server via SSH)

1. 🔍 [SSH](https://www.digitalocean.com/community/tutorials/how-to-use-ssh-to-connect-to-a-remote-server) into your server and navigate to the `Mods` folder on your server
    - if you installed 7 Days to Die with [LinuxGSM](https://linuxgsm.com/servers/sdtdserver/) (which I'd highly recommend), the default mods folder will be under `~/serverfiles/Mods` (which you may have to create)
2. 📦 Download the latest `7dtd-discord-log-hook.zip` release from [this link](https://github.com/jonathan-robertson/7dtd-discord-log-hook/releases/latest/) with whatever tool you prefer
    - example: `wget https://github.com/jonathan-robertson/7dtd-discord-log-hook/releases/latest/download/7dtd-discord-log-hook.zip`
3. 📂 Unzip this file to a folder by the same name: `unzip 7dtd-discord-log-hook.zip -d 7dtd-discord-log-hook`
    - you may need to install `unzip` if it isn't already installed: `sudo apt-get update && sudo apt-get install unzip`
    - once unzipped, you can remove the 7dtd-discord-log-hook download with `rm 7dtd-discord-log-hook.zip`
4. ♻️ Restart your server to allow this mod to take effect and monitor your logs to ensure it starts successfully:
    - you can search the logs for the word `DiscordLogHook`; the name of this mod will appear with that phrase and all log lines it produces will be presented with this prefix for quick reference
    - rather than monitoring telnet, I'd recommend viewing the console logs directly because mod and DLL registration happens very early in the startup process and you may miss it if you connect via telnet after this happens
    - you can reference your server config file to identify your logs folder
    - if you installed 7 Days to Die with [LinuxGSM](https://linuxgsm.com/servers/sdtdserver/), your console log will be under `log/console/sdtdserver-console.log`
    - I'd highly recommend using `less` to open this file for a variety of reasons: it's safe to view active files with, easy to search, and can be automatically tailed/followed by pressing a keyboard shortcut so you can monitor logs in realtime
      - follow: `SHIFT+F` (use `CTRL+C` to exit follow mode)
      - exit: `q` to exit less when not in follow mode
      - search: `/DiscordLogHook` [enter] to enter search mode for the lines that will be produced by this mod; while in search mode, use `n` to navigate to the next match or `SHIFT+n` to navigate to the previous match

### Create a Discord Webhook

1. [Make a private channel](https://discord.com/blog/starting-your-first-discord-server) in a Discord Server you manage
2. [Create a Webhook](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks) in this channel
3. In Webhook Settings, click to `Copy Webhook URL`
4. In your game server (as an admin), run `dlh test URL` (where URL is the webhook url you copied)
   - you should see a new message in your discord server letting you know that the request was successful (if you do not, then you did something wrong; carefully re-read the steps above to try again)
5. Once confirmed, you can now add your log hook `dlh add log URL` (where URL is the webhook url you copied)
6. In the same channel (or another), you can setup server status notifications with `dlh add status URL` (where URL is the webhook url you copied)
   - this can help both you and your players know when your server is restarting (or if it crashed) and when it comes back up again
7. There are more options and settings available in this tool; check out the [Commands](#commands) section above, or run `dlh help` from the in-game console
