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

        [HarmonyPrefix]
        public static bool ChaosSpawnPatch(ref Queue<global::RoleType> queueToFill, int playersToSpawn)
        {
            if (!generalizedSpawner(ref queueToFill, playersToSpawn, "Chaos_Config", SpawnWaveControl.early_config.ChaosSpawnWaveRules))
            {
                LoggerTool.log_msg_static("Letting original method run");
                return true;
            }
            LoggerTool.log_msg_static("Not letting original method run");
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

        /// <summary>
        /// While thinking about whether I want to generalize the above approach, I am reminded of the story of ropes and users. 
        /// This is more of a proof of concept or a potential solution for later. 
        /// </summary>
        /// <param name="queueToFill"></param>
        /// <param name="playersToSpawn"></param>
        /// <param name="spawnType"></param>
        /// <param name="config_keys"></param>
        /// <returns></returns>
        public static bool generalizedSpawner(ref Queue<global::RoleType> queueToFill, int playersToSpawn, String spawnType, Dictionary<string, float> config_keys)
        {
            SpawnWaveControl.early_config.ProgramLevel.TryGetValue(spawnType, out bool is_enabled);

            if (!is_enabled)
            {
                //Runs the normal code execution
                return false;
            }


            if (config_keys == null || config_keys.Count == 0)
            {
                return false;
            }


            int total_spawned = 0;

            List<RoleType> roles_to_modify = new List<RoleType>();
            Dictionary<RoleType, int> role_probability_pair = new Dictionary<RoleType, int>();

            foreach (KeyValuePair<string, float> paired_entry in config_keys)
            {
                RoleType associated_role = (RoleType)Enum.Parse(typeof(RoleType), paired_entry.Key);
                if (paired_entry.Value > 0)
                {
                    int players_to_queue = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
                    role_probability_pair.Add(associated_role, players_to_queue < 1 ? 1 : players_to_queue);
                }
            }

            foreach (KeyValuePair<RoleType, int> paired_spawn in role_probability_pair)
            {
                if (AddNewPlayerToQueue(paired_spawn.Value, ref total_spawned, paired_spawn.Key, playersToSpawn, queueToFill))
                {
                    LoggerTool.log_msg_static($"Alright, what was the probabilities  { paired_spawn.Key} and its prob {paired_spawn.Value}");
                }
                else
                {
                    LoggerTool.log_msg_static($"Alright, what was the probabilities of non-added  { paired_spawn.Key} and its prob {paired_spawn.Value}");
                }

            }

            return true;
        }



        private static bool AddNewPlayerToQueue(int amount_to_spawn, ref int total_spawned, RoleType role_to_spawn, int playersToSpawn, Queue<global::RoleType> queueToFill)
        {

            for (int chaos_to_spawn_counter = 0; chaos_to_spawn_counter < amount_to_spawn; chaos_to_spawn_counter++)
            {
                if (total_spawned >= playersToSpawn)
                {
                    return false;
                }
                queueToFill.Enqueue(role_to_spawn);
                total_spawned += 1;
            }
            return true;
        }


    }
}
