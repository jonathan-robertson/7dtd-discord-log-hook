using DiscordLogHook.Data;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace DiscordLogHook.Utilities {
    internal class DiscordLogger {
        internal static Settings Settings { get; private set; }
        internal static RollingQueue RollingQueue { get; private set; }

        internal static void Init() {
            Settings = SettingsManager.Load();
            RollingQueue = new RollingQueue(Settings.rollingLimit);
        }

        private DiscordLogger() { }

        internal static void LogCallbackDelegate(string _msg, string _trace, LogType _type) {
            if (_type == LogType.Log || Settings.loggerIgnorelist.Where(item => _msg.Contains(item)).Any()) {
                if (Settings.loggerWebhooks.Count > 0) {
                    RollingQueue.Add(_msg);
                }
                return;
            }

            switch (_type) {
                case LogType.Warning:
                    Settings.loggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, Payload.Warn(_msg, RollingQueue.GetLines()).Serialize())));
                    return;
                case LogType.Error:
                    Settings.loggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, Payload.Err(_msg, RollingQueue.GetLines()).Serialize())));
                    return;
                case LogType.Exception:
                    Settings.loggerWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, Payload.Err(_msg, _trace, RollingQueue.GetLines()).Serialize())));
                    return;
            }
        }

        internal static void OnGameAwake() {
            Settings.statusWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, Payload.Info(Settings.messageOnGameAwake).Serialize())));
        }

        internal static void OnGameStartDone() {
            Settings.statusWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, Payload.Info(Settings.messageOnGameStartDone).Serialize())));
        }

        internal static void OnGameShutdown() {
            Settings.statusWebhooks.ForEach(url => ThreadManager.StartCoroutine(Send(url, Payload.Info(Settings.messageOnGameShutdown).Serialize())));
        }

        /**
         * <summary>Send a payload to the provided webhook url.</summary>
         * <param name="url">Destination to send the provided payload to.</param>
         * <param name="body">Content to send to the provided webhook url.</param>
         * <returns>Enumerator to support Coroutine.</returns>
         * <remarks>, Action<string> onSuccess = null, Action<Exception> onFailure = null</remarks>
         */
        internal static IEnumerator Send(string url, string body) {
            using (UnityWebRequest request = UnityWebRequest.Post(url, body)) {
                request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(body));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("cache-control", "no-cache");
                yield return request.SendWebRequest();
                // NOTE: intentionally ignoring response for faster logging... and because it would cause control loops
                /*
                if (request.result != UnityWebRequest.Result.Success) {
                    onFailure?.Invoke(new Exception($"Request failed with web result of {request.result}"));
                } else {
                    if (onSuccess != null) {
                        try {
                            while (!request.downloadHandler.isDone) { }
                            // TODO: shouldn't there be a timeout on this?
                            onSuccess(request.downloadHandler.text);
                        } catch (Exception e) {
                            //log.Warn("Failed to finish downloading response body", e);
                            onFailure?.Invoke(e);
                        }
                    }
                }
                */
            }
        }
    }
}
