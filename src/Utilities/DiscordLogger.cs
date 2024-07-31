using DiscordLogHook.Data;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DiscordLogHook.Utilities
{
    internal class DiscordLogger
    {
        private static readonly ModLog<DiscordLogger> _log = new ModLog<DiscordLogger>();

        public static Settings Settings { get; private set; }
        internal static RollingQueue RollingQueue { get; private set; }

        internal static void Init()
        {
            Settings = SettingsManager.Load();
            RollingQueue = new RollingQueue(Settings.RollingLimit);
        }

        private DiscordLogger() { }

        internal static void LogCallbackDelegate(string _msg, string _trace, LogType _type)
        {
            if (_type > Settings.GetLogLevel() || Settings.LoggerIgnorelist.Where(item => _msg.Contains(item)).Any())
            {
                if (Settings.LoggerWebhooks.Count > 0)
                {
                    RollingQueue.Add(_msg);
                }
                return;
            }

            switch (_type)
            {
                case LogType.Log:
                    var infoContent = Payload.Info(_msg, RollingQueue.GetLines()).Serialize();
                    Settings.LoggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, infoContent)));
                    return;
                case LogType.Warning:
                    var warnContent = Payload.Warn(_msg, RollingQueue.GetLines()).Serialize();
                    Settings.LoggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, warnContent)));
                    return;
                case LogType.Error:
                    var errContent = Payload.Err(_msg, RollingQueue.GetLines()).Serialize();
                    Settings.LoggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, errContent)));
                    return;
                case LogType.Exception:
                    var excContent = Payload.Err(_msg, _trace, RollingQueue.GetLines()).Serialize();
                    Settings.LoggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, excContent)));
                    return;
            }
        }

        internal static void OnGameAwake()
        {
            var content = Payload.Content(Settings.GetMessageForAwake()).Serialize();
            Settings.StatusWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, content)));

            // Note: Register here instead of in InitMod to ensure this is the final handler registered to the GameStartDone delegate
            ModEvents.GameStartDone.RegisterHandler(OnGameStartTrulyDone);
        }

        internal static void OnGameStartTrulyDone()
        {
            var content = Payload.Content(Settings.GetMessageForStartDone()).Serialize();
            Settings.StatusWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, content)));
        }

        internal static void OnGameShutdown()
        {
            var content = Payload.Content(Settings.GetMessageForShutdown()).Serialize();
            Settings.StatusWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, content)));
        }

        /**
         * <summary>Send a payload to the provided webhook url.</summary>
         * <param name="url">Destination to send the provided payload to.</param>
         * <param name="body">Content to send to the provided webhook url.</param>
         * <returns>Enumerator to support Coroutine.</returns>
         * <remarks>, Action<string> onSuccess = null, Action<Exception> onFailure = null</remarks>
         */
        internal static IEnumerator Send(string url, string body)
        {
            _log.Trace($"attempting to send...\nurl: {url}\nbody: {body}");
            using (var request = UnityWebRequest.Post(url, body, "application/json"))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    _log.Debug($"request did not succeed: {request.result} | {request.responseCode}");
                }
                else
                {
                    _log.Trace("request succeeded");
                }
            }
        }
    }
}
