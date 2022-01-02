using HarmonyLib;
using Respawning;
using SpawnWaveControl.Utilities;
using System.Collections.Generic;

namespace SpawnWaveControl.Patches
{

    [HarmonyPatch(typeof(NineTailedFoxSpawnHandler))]
    [HarmonyPatch(nameof(NineTailedFoxSpawnHandler.GenerateQueue), MethodType.Normal)]
    public class MtfSpawnPatcher
    {
        [HarmonyPrefix]
        public static bool MtfSpawnPatch(ref Queue<global::RoleType> queueToFill, int playersToSpawn)
        {
            if (!generalizedSpawner.GeneralizedSpawner(ref queueToFill, playersToSpawn, "Mtf_Config", SpawnWaveControl.early_config.MtfSpawnWaveRules))
            {
                LoggerTool.log_msg_static("Letting MtfSpawnPatch original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not MtfSpawnPatch letting original method run");
            return false;
        }
    }
}
