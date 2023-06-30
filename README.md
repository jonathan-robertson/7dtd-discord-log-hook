# Discord Log Hook

[![🧪 Tested On](https://img.shields.io/badge/🧪%20Tested%20On-A21%20b324-blue.svg)](https://7daystodie.com/) [![📦 Automated Release](https://github.com/jonathan-robertson/7dtd-discord-log-hook/actions/workflows/release.yml/badge.svg)](https://github.com/jonathan-robertson/7dtd-discord-log-hook/actions/workflows/release.yml)

- [Discord Log Hook](#discord-log-hook)
  - [Features](#features)
  - [Commands](#commands)

Configurable log hook into Discord for game service monitoring.

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
