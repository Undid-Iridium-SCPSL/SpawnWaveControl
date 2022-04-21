using HarmonyLib;
using Respawning;
using SpawnWaveControl.Utilities;
using System;
using System.Collections.Generic;


namespace SpawnWaveControl.Patches
{
    [HarmonyPatch(typeof(ChaosInsurgencySpawnHandler))]
    [HarmonyPatch(nameof(ChaosInsurgencySpawnHandler.GenerateQueue), MethodType.Normal)]
    public class ChaosSpawnPatcher
    {

        private static char[] chaos_probability_range = null;

        [HarmonyPrefix]
        public static bool ChaosSpawnPatch(ref Queue<global::RoleType> queueToFill, int playersToSpawn)
        {
            SpawnWaveControl.Instance.Config.ProgramLevel.TryGetValue("Chaos_Config", out bool is_enabled);

            if (!is_enabled)
            {
                //Runs the normal code execution
                return true;
            }


            Dictionary<RoleType, float> config_keys = SpawnWaveControl.Instance.Config.ChaosSpawnWaveRules;
            bool probability_enabled = SpawnWaveControl.Instance.Config.probability_flag;
            Dictionary<char, RoleType> associated_pair_key = new Dictionary<char, RoleType>();

            LoggerTool.log_msg_static($"What was probability_enabled {probability_enabled}");
            //To be fair I could make this a shared function but only if there were more than two teams would I care to. 
            if (probability_enabled)
            {
                LoggerTool.log_msg_static("Loading probabilty logic");
                //This is what is going to allow us to reduce a O(N) O(N+K) at best search (potentially even O(N log N), O(N+K) to O(2*N) O(100) -> O(N), 100
                if (chaos_probability_range == null)
                {
                    chaos_probability_range = new char[100];
                }

                int prev_pos = 0; //Start pos to start changing chars from
                int max = 100; //Max % being 100
                int start_char = 65; //A

                foreach (KeyValuePair<RoleType, float> paired_data in config_keys)
                {
                    int prev_start = prev_pos;
                    prev_pos += (int)Math.Floor(paired_data.Value * 100);
                    for (int pos = prev_start; pos < prev_pos && pos <= max; pos++)
                    {
                        chaos_probability_range[pos] = (char)start_char;
                    }
                    associated_pair_key.Add((char)start_char, paired_data.Key);
                    start_char += 1;//A->B->C
                }
            }


            if (!generalizedSpawner.GeneralizedSpawner(ref queueToFill, playersToSpawn, config_keys, probability_enabled, chaos_probability_range, associated_pair_key))
            {
                LoggerTool.log_msg_static("Letting ChaosSpawnPatch original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not letting ChaosSpawnPatch original method run");
            return false;

        }



    }
}
