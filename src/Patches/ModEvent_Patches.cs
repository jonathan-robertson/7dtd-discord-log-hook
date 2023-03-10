using DiscordLogHook.Utilities;
using HarmonyLib;
using System;

namespace DiscordLogHook.Patches
{
    [HarmonyPatch(typeof(ModEvent), "Invoke")]
    internal class ModEvent_Invoke_Patches
    {
        private static readonly ModLog<ModEvent_Invoke_Patches> _log = new ModLog<ModEvent_Invoke_Patches>();

        public static void Postfix(ModEvent __instance)
        {
            try
            {
                if (__instance.eventName.Equals("GameStartDone"))
                {
                    DiscordLogger.OnGameStartTrulyDone();
                }
            }
            catch (Exception e)
            {
                _log.Error("Failed Postfix", e);
            }
        }
    }
}
