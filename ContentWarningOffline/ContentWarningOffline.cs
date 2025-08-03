using System.Reflection;
using BepInEx;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using System.Collections;

namespace ContentWarningOffline
{
    [BepInPlugin("kirigiri.contentwarning.offline", "ContentWarningOffline", "1.0.0.0")]
    public class ContentWarningOffline : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo(@"
 ██ ▄█▀ ██▓ ██▀███   ██▓  ▄████  ██▓ ██▀███   ██▓
 ██▄█▒ ▓██▒▓██ ▒ ██▒▓██▒ ██▒ ▀█▒▓██▒▓██ ▒ ██▒▓██▒
▓███▄░ ▒██▒▓██ ░▄█ ▒▒██▒▒██░▄▄▄░▒██▒▓██ ░▄█ ▒▒██▒
▓██ █▄ ░██░▒██▀▀█▄  ░██░░▓█  ██▓░██░▒██▀▀█▄  ░██░
▒██▒ █▄░██░░██▓ ▒██▒░██░░▒▓███▀▒░██░░██▓ ▒██▒░██░
▒ ▒▒ ▓▒░▓  ░ ▒▓ ░▒▓░░▓   ░▒   ▒ ░▓  ░ ▒▓ ░▒▓░░▓  
░ ░▒ ▒░ ▒ ░  ░▒ ░ ▒░ ▒ ░  ░   ░  ▒ ░  ░▒ ░ ▒░ ▒ ░
░ ░░ ░  ▒ ░  ░░   ░  ▒ ░░ ░   ░  ▒ ░  ░░   ░  ▒ ░
░  ░    ░     ░      ░        ░  ░     ░      ░  
                                                 
");

            var harmony = new Harmony("kirigiri.contentwarning.offline");
            harmony.PatchAll();

            Logger.LogInfo("Made with <3 By Kirigiri \nhttps://discord.gg/TBs8Te5nwn");
        }

        [HarmonyPatch(typeof(MainMenuHandler), "ConnectToPhoton")]
        public class ConnectToPhotonPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(MainMenuHandler __instance)
            {
                PhotonNetwork.OfflineMode = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(CheckVersionHandler), "CheckVersionCoroutine")]
        public class CheckVersionPatcher
        {
            [HarmonyPrefix]
            public static bool Prefix(CheckVersionHandler __instance, ref IEnumerator __result)
            {
                __result = ForcedCoroutine(__instance);
                return false;
            }

            private static IEnumerator ForcedCoroutine(CheckVersionHandler instance)
            {
                string forcedResponse = "VersionOK";

                MethodInfo checkResultMethod = typeof(CheckVersionHandler)
                    .GetMethod("CheckResult", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (checkResultMethod != null)
                {
                    checkResultMethod.Invoke(instance, new object[] { forcedResponse });
                }
                else
                {
                    Debug.LogError("Could not find 'CheckResult' method via reflection.");
                }

                yield break;
            }
        }

        [HarmonyPatch(typeof(MainMenuHandler), nameof(MainMenuHandler.JoinRandom))]
        public class JoinRandomPatch
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {

                Modal.Show(
                    "<color=purple>This cannot be used in this cracked version</color>",
                    "This feature is not available in the current version of the application.",
                    new ModalOption[]
                    {
                new ModalOption("OK", null)
                    },
                    () => { }
                );

                return false;
            }
        }
    }
}
