using DiscordLogHook.Data;
using DiscordLogHook.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscordLogHook
{
    [TestClass]
    public class Tests
    {
        private static readonly List<string> _statusWebhooks = new List<string>()
        {
            "https://discord.com/api/webhooks/EXAMPLE921667698718/Xs8eGL8EehVVlg7S0b70AEi8tJZsP3TSR1YWp11JqDhyh8GnnIUYEv2fMC-0CxXZlONE",
            "https://discord.com/api/webhooks/EXAMPLE921667698718/Xs8eGL8EehVVlg7S0b70AEi8tJZsP3TSR1YWp11JqDhyh8GnnIUYEv2fMC-0CxXZlTWO"
        };
        private static readonly List<string> _loggerWebhooks = new List<string>()
        {
            "https://discord.com/api/webhooks/EXAMPLE293306499092/exlg8bueo9eaeH2uElpDabbJpBNM5qwNPftzmaPT8WTq3QXiTdUnwVtl06DGd-us9ONE",
            "https://discord.com/api/webhooks/EXAMPLE293306499092/exlg8bueo9eaeH2uElpDabbJpBNM5qwNPftzmaPT8WTq3QXiTdUnwVtl06DGd-us9TWO"
        };
        private static readonly List<string> _ignoreList = new List<string>()
        {
            "RR SSE (log): SocketError (Shutdown) while trying to write",
            "ERR [EOS] VerifyIdToken failure: Unknown platform:",
            "should be a parent but is not!",
            "Failed with invalid user",
            "InvalidUser",
            "ERR NCSimple_Deserializer"
        };
        private static readonly string _input = $"{{\"StatusWebhooks\":[\"{string.Join("\",\"", _statusWebhooks)}\"],\"RollingLimit\":6,\"LogLevel\":1,\"LoggerWebhooks\":[\"{string.Join("\",\"", _loggerWebhooks)}\"],\"LoggerIgnorelist\":[\"{string.Join("\",\"", _ignoreList)}\"],\"MessageOnGameShutdown\":\"aasdfgHJkL;\",\"MessageOnGameAwake\":\"\",\"MessageOnGameStartDone\":\"\"}}";
        private static readonly Settings _settings = Json<Settings>.Deserialize(_input);

        [TestMethod]
        public void TestGeneratedEqualsRaw()
        {
            var raw = "{\"StatusWebhooks\":[\"https://discord.com/api/webhooks/EXAMPLE921667698718/Xs8eGL8EehVVlg7S0b70AEi8tJZsP3TSR1YWp11JqDhyh8GnnIUYEv2fMC-0CxXZlONE\",\"https://discord.com/api/webhooks/EXAMPLE921667698718/Xs8eGL8EehVVlg7S0b70AEi8tJZsP3TSR1YWp11JqDhyh8GnnIUYEv2fMC-0CxXZlTWO\"],\"RollingLimit\":6,\"LogLevel\":1,\"LoggerWebhooks\":[\"https://discord.com/api/webhooks/EXAMPLE293306499092/exlg8bueo9eaeH2uElpDabbJpBNM5qwNPftzmaPT8WTq3QXiTdUnwVtl06DGd-us9ONE\",\"https://discord.com/api/webhooks/EXAMPLE293306499092/exlg8bueo9eaeH2uElpDabbJpBNM5qwNPftzmaPT8WTq3QXiTdUnwVtl06DGd-us9TWO\"],\"LoggerIgnorelist\":[\"RR SSE (log): SocketError (Shutdown) while trying to write\",\"ERR [EOS] VerifyIdToken failure: Unknown platform:\",\"should be a parent but is not!\",\"Failed with invalid user\",\"InvalidUser\",\"ERR NCSimple_Deserializer\"],\"MessageOnGameShutdown\":\"aasdfgHJkL;\",\"MessageOnGameAwake\":\"\",\"MessageOnGameStartDone\":\"\"}";

            Assert.AreEqual(raw, _input);
        }

        [TestMethod]
        public void TestLoadStatusWebhooks()
        {
            Assert.AreEqual(_statusWebhooks.Count, _settings.StatusWebhooks.Count);
            for (var i = 0; i < _statusWebhooks.Count; i++)
            {
                Assert.AreEqual(_statusWebhooks[i], _settings.StatusWebhooks[i]);
            }
        }

        [TestMethod]
        public void TestLoadRollingLimit()
        {
            Assert.AreEqual(6, _settings.RollingLimit);
        }

        [TestMethod]
        public void TestLoadLogLevel()
        {
            Assert.AreEqual(1, _settings.LogLevel);
        }

        [TestMethod]
        public void TestLoadLoggerWebhooks()
        {
            Assert.AreEqual(_loggerWebhooks.Count, _settings.LoggerWebhooks.Count);
            for (var i = 0; i < _loggerWebhooks.Count; i++)
            {
                Assert.AreEqual(_loggerWebhooks[i], _settings.LoggerWebhooks[i]);
            }
        }

        [TestMethod]
        public void TestLoadLoggerIgnorelist()
        {
            Assert.AreEqual(_ignoreList.Count, _settings.LoggerIgnorelist.Count);
            for (var i = 0; i < _ignoreList.Count; i++)
            {
                Assert.AreEqual(_ignoreList[i], _settings.LoggerIgnorelist[i]);
            }
        }

        [TestMethod]
        public void TestLoadLoggerMessageOnGameCollection()
        {
            Assert.AreEqual("aasdfgHJkL;", _settings.MessageOnGameShutdown);
            Assert.AreEqual("", _settings.MessageOnGameAwake);
            Assert.AreEqual("", _settings.MessageOnGameStartDone);
        }

        [TestMethod]
        public void TestRegex()
        {
            //var message = "2024-07-31T06:57:32 87.753 INF Test info.";
            //var _logPattern = Regex.Escape(@"^([\d-T: .]*) (\w)* (.*)$");
            //var matches = Regex.Matches(message, _logPattern, RegexOptions.IgnorePatternWhitespace);
            //Assert.AreEqual(3, matches.Count);
            //var _explicitLogPattern = Regex.Escape("^(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2} \\d\\.\\d{3}) (INF|WRN|ERR|EXC) (.*)$");



            var pattern = @"^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2} \d*\.\d{3}) (INF|WRN|ERR|EXC) (.*)";
            //var pattern = @"^([\d-T: .]*) (\w)* (.*)$";
            //var input = @"2024-07-31T06:23:55 4.970 WRN Couldn't create a Convex Mesh from source mesh ""Head"" within the maximum polygons limit (256). The partial hull will be used. Consider simplifying your mesh.";
            var input = "2024-07-31T06:57:32 87.753 INF Test info.";
            var matches = Regex.Matches(input, pattern, RegexOptions.Multiline);

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(4, matches[0].Groups.Count);
            Assert.AreEqual("2024-07-31T06:57:32 87.753", matches[0].Groups[1].Value);
            Assert.AreEqual("INF", matches[0].Groups[2].Value);
            Assert.AreEqual("Test info.", matches[0].Groups[3].Value);

            var content = "";
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
            Assert.AreEqual("ℹ️ Test info.\n> 2024-07-31T06:57:32 87.753", content);
        }

        [TestMethod]
        public void TestSetContent()
        {
            var lines = new List<string>()
            {
                "2024-07-31T06:57:32 87.737 INF Dymesh: Mesh location: C:\\Users\\user\\AppData\\Roaming/7DaysToDie\\Saves/World/Game/DynamicMeshes/",
                "2024-07-31T06:57:32 87.738 INF Dymesh: Loading Items: C:\\Users\\user\\AppData\\Roaming/7DaysToDie\\Saves/World/Game/DynamicMeshes/",
                "2024-07-31T06:57:32 87.750 INF Dymesh: Loaded Items: 1",
                "2024-07-31T06:57:32 87.750 INF Dymesh: Loading all items took: 0.0120208 seconds.",
                "2024-07-31T06:57:32 87.753 INF Clearing queues.",
                "2024-07-31T06:57:32 87.753 INF Cleared queues.",
                "2024-07-31T06:57:32 87.754 INF Dynamic thread starting",
                "2024-07-31T06:57:32 87.757 INF Dymesh door replacement: imposterBlock",
                "2024-07-31T06:57:34 90.282 INF Executing command 'exception' by Terminal Window",
            };
            var trace = "  at CommandExtensions.Commands.ConsoleCmdException.Execute (System.Collections.Generic.List`1[T] _params, CommandSenderInfo _senderInfo) [0x00022] in <5a4190d3fa7d49858bf72d271fb8b84f>:0\r\n  at (wrapper dynamic-method) SdtdConsole.DMD<System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] SdtdConsole:executeCommand(System.String, CommandSenderInfo)>(SdtdConsole,string,CommandSenderInfo)\r\nUnityEngine.StackTraceUtility:ExtractStringFromException(Object)\r\nLog:Exception(Exception)\r\nSdtdConsole:DMD<System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] SdtdConsole:executeCommand(System.String, CommandSenderInfo)>(SdtdConsole, String, CommandSenderInfo)\r\nMonoMod.Utils.DynamicMethodDefinition:SyncProxy<System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] SdtdConsole:executeCommand(System.String, CommandSenderInfo)>(SdtdConsole, String, CommandSenderInfo)\r\nSdtdConsole:Update()\r\n";

            var info = Payload.Info("2024-07-31T06:57:32 87.753 INF Test info.", lines);
            Assert.AreEqual("ℹ️ Test info.\n> 2024-07-31T06:57:32 87.753", info.content);
            var warn = Payload.Warn("2024-07-31T06:57:34 90.283 WRN Test warning.", lines);
            Assert.AreEqual("⚠️ Test warning.\n> 2024-07-31T06:57:34 90.283", warn.content);
            var err = Payload.Err("2024-07-31T06:57:34 90.283 ERR Test error.", lines);
            Assert.AreEqual("📟 Test error.\n> 2024-07-31T06:57:34 90.283", err.content);
            var exc = Payload.Err("2024-07-31T06:00:29 81.339 EXC Test exception.", trace, lines);
            Assert.AreEqual("🚨 Test exception.\n> 2024-07-31T06:00:29 81.339", exc.content);
        }
    }
}
