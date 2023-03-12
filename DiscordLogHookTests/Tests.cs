using DiscordLogHook.Data;
using DiscordLogHook.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
    }
}
