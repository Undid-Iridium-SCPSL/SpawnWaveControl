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
            if (!generalizedSpawner.GeneralizedSpawner(ref queueToFill, playersToSpawn, "Chaos_Config", SpawnWaveControl.early_config.ChaosSpawnWaveRules))
            {
                LoggerTool.log_msg_static("Letting ChaosSpawnPatch original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not letting ChaosSpawnPatch original method run");
            return false;

            //    SpawnWaveControl.early_config.ProgramLevel.TryGetValue("Chaos_Config", out bool is_enabled);

            //if (!is_enabled)
            //{
            //    //Runs the normal code execution
            //    return true;
            //}

            //Dictionary<string, float> config_keys = SpawnWaveControl.early_config.ChaosSpawnWaveRules;
            //if (config_keys == null || config_keys.Count == 0)
            //{
            //    return true;
            //}



            ////Default logic which is meh
            //int marauder_prob = (int)Math.Floor((float)playersToSpawn * 0.2f);
            //int repressor_prob = (int)Math.Floor((float)(playersToSpawn - marauder_prob) * 0.3f);
            //int rifleman_prob = (int)Math.Floor((float)(playersToSpawn - repressor_prob - marauder_prob));
            //int total_spawned = 0;

            //List<RoleType> roles_to_modify = new List<RoleType>();
            //Dictionary<RoleType, int> role_probability_pair = new Dictionary<RoleType, int>();

            //foreach (KeyValuePair<string, float> paired_entry in config_keys)
            //{
            //    RoleType associated_role = (RoleType)Enum.Parse(typeof(RoleType), paired_entry.Key);

            //    if (associated_role == RoleType.ChaosMarauder)
            //    {
            //        if (paired_entry.Value > 0)
            //        {
            //            marauder_prob = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
            //        }
            //    }
            //    else if (associated_role != RoleType.ChaosRepressor)
            //    {
            //        if (paired_entry.Value > 0)
            //        {
            //            repressor_prob = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
            //        }
            //    }
            //    else if (associated_role != RoleType.ChaosRifleman)
            //    {
            //        if (paired_entry.Value > 0)
            //        {
            //            rifleman_prob = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
            //        }
            //    }
            //}

            //if (!AddNewPlayerToQueue(marauder_prob, ref total_spawned, global::RoleType.ChaosMarauder, playersToSpawn, queueToFill))
            //{
            //    //Have to remove all the ones we just added because we got outside the range of allowed spawns, aka if we had 1.5% instead of .5%
            //    //We would go overboard and try to spawn more players than we should. 
            //    for (int pos = 0; pos < total_spawned; pos++)
            //    {
            //        queueToFill.Dequeue();
            //    }
            //    return true;
            //}
            //if (!AddNewPlayerToQueue(repressor_prob, ref total_spawned, global::RoleType.ChaosRepressor, playersToSpawn, queueToFill))
            //{
            //    for (int pos = 0; pos < total_spawned; pos++)
            //    {
            //        queueToFill.Dequeue();
            //    }
            //    return true;
            //}
            //if (!AddNewPlayerToQueue(rifleman_prob, ref total_spawned, global::RoleType.ChaosRifleman, playersToSpawn, queueToFill))
            //{
            //    for (int pos = 0; pos < total_spawned; pos++)
            //    {
            //        queueToFill.Dequeue();
            //    }
            //    return true;
            //}

            //LoggerTool.log_msg_static($"Alright, what was the probabilities " +
            //    $"marauder_prob: {marauder_prob}" +
            //    $"repressor_prob: {repressor_prob}" +
            //    $"rifleman_prob: {rifleman_prob}"
            //    );


        }



    }
}
