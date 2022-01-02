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
            SpawnWaveControl.early_config.ProgramLevel.TryGetValue("Mtf_Config", out bool is_enabled);

            if (!is_enabled)
            {
                //Runs the normal code execution
                LoggerTool.log_msg_static("We were not enabled. Letting original method run");
                return true;
            }

            if (!generalizedSpawner.GeneralizedSpawner(ref queueToFill, playersToSpawn, SpawnWaveControl.early_config.MtfSpawnWaveRules))
            {
                LoggerTool.log_msg_static("Letting MtfSpawnPatch original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not MtfSpawnPatch letting original method run");
            return false;
        }
    }
}
