using HarmonyLib;
using Respawning;
using SpawnWaveControl.Utilities;
using System.Collections.Generic;


namespace SpawnWaveControl.Patches
{
    [HarmonyPatch(typeof(ChaosInsurgencySpawnHandler))]
    [HarmonyPatch(nameof(ChaosInsurgencySpawnHandler.GenerateQueue), MethodType.Normal)]
    public class ChaosSpawnPatcher
    {

        [HarmonyPrefix]
        public static bool ChaosSpawnPatch(ref Queue<global::RoleType> queueToFill, int playersToSpawn)
        {
            SpawnWaveControl.early_config.ProgramLevel.TryGetValue("Chaos_Config", out bool is_enabled);

            if (!is_enabled)
            {
                //Runs the normal code execution
                return true;
            }

            if (!generalizedSpawner.GeneralizedSpawner(ref queueToFill, playersToSpawn, SpawnWaveControl.early_config.ChaosSpawnWaveRules))
            {
                LoggerTool.log_msg_static("Letting ChaosSpawnPatch original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not letting ChaosSpawnPatch original method run");
            return false;

        }



    }
}
