using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.Rendering.RayTracingAccelerationStructure;

namespace BetterCogworkDancers
{

    [BepInPlugin("io.github.sstestmod3", "BetterCogworkDancers", "1.0.0")]
    public class BetterCogworkDancersPlugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        private void Awake()
        {

            Harmony.CreateAndPatchAll(typeof(DancerPatch), null);
            Log = base.Logger;
            Log.LogInfo($"Plugin {Info.Metadata.Name} {Info.Metadata.Version} has loaded!");
        }
    }

    internal static class DancerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayMakerFSM), "Start")]
        private static void ModifyDance(PlayMakerFSM __instance)
        {
                if (__instance.name == "Dancer A" && __instance.FsmName == "Control" &&
                    __instance.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                BetterCogworkDancersPlugin.Log.LogInfo("Modifying Dancer A");

                    if (__instance.gameObject.GetComponent<DancePatch>() == null)
                    {
                        __instance.gameObject.AddComponent<DancePatch>();
                    }

                }
            }
        }

}