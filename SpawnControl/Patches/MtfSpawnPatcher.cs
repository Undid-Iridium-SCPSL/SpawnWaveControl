using HarmonyLib;
using Respawning;
using SpawnWaveControl.Utilities;
using System;
using System.Collections.Generic;

namespace SpawnWaveControl.Patches
{

    [HarmonyPatch(typeof(NineTailedFoxSpawnHandler))]
    [HarmonyPatch(nameof(NineTailedFoxSpawnHandler.GenerateQueue), MethodType.Normal)]
    public class MtfSpawnPatcher
    {

        private static char[] mtf_probability_range = null;


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

            Dictionary<RoleType, float> config_keys = SpawnWaveControl.early_config.MtfSpawnWaveRules;
            bool probability_enabled = SpawnWaveControl.early_config.probability_flag;
            Dictionary<char, RoleType> associated_pair_key = new Dictionary<char, RoleType>();

            if (probability_enabled)
            {
                //This is what is going to allow us to reduce a O(N) O(N+K) at best search (potentially even O(N log N), O(N+K) to O(2*N) O(100) -> O(N), 100
                if (mtf_probability_range == null)
                {
                    mtf_probability_range = new char[100];
                }

                int start = 0;
                int max = 100;
                int start_char = 65; //A

                foreach (KeyValuePair<RoleType, float> paired_data in config_keys)
                {
                    int prev_start = start;
                    start += (int)Math.Floor(paired_data.Value * 100);
                    for (int pos = prev_start; pos < max; pos++)
                    {
                        mtf_probability_range[pos] = (char)start_char;
                    }
                    associated_pair_key.Add((char)start_char, paired_data.Key);
                    start_char += 1;
                }
            }

            if (!generalizedSpawner.GeneralizedSpawner(ref queueToFill, playersToSpawn, config_keys, probability_enabled, mtf_probability_range, associated_pair_key))
            {
                LoggerTool.log_msg_static("Letting MtfSpawnPatch original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not MtfSpawnPatch letting original method run");
            return false;
        }
    }
}
