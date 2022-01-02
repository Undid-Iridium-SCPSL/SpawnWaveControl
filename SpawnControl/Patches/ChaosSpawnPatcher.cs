using HarmonyLib;
using Respawning;
using System;
using System.Collections.Generic;

namespace SpawnWaveControl.Patches
{
    [HarmonyPatch(typeof(ChaosInsurgencySpawnHandler))]
    [HarmonyPatch("ForceReload", MethodType.Normal)]
    public class ChaosSpawnPatcher
    {

        [HarmonyPrefix]
        public static bool ChaosSpawnPatch(Queue<global::RoleType> queueToFill, int playersToSpawn)
        {
            SpawnWaveControl.early_config.ProgramLevel.TryGetValue("Keycard_Config", out bool is_enabled);

            if (!is_enabled)
            {
                //Runs the normal code execution
                return true;
            }

            Dictionary<string, float> config_keys = SpawnWaveControl.early_config.ChaosSpawnWaveRules;
            if (config_keys == null || config_keys.Count == 0)
            {
                return true;
            }

            //Default logic which is meh
            int marauder_prob = (int)Math.Floor((float)playersToSpawn * 0.2f);
            int repressor_prob = (int)Math.Floor((float)(playersToSpawn - marauder_prob) * 0.3f);
            int rifleman_prob = (int)Math.Floor((float)(playersToSpawn - repressor_prob - marauder_prob));
            int total_spawned = 0;

            foreach (KeyValuePair<string, float> paired_entry in config_keys)
            {
                RoleType associated_role = (RoleType)Enum.Parse(typeof(RoleType), paired_entry.Key);
                if (associated_role == RoleType.ChaosMarauder)
                {
                    if (paired_entry.Value > 0)
                    {
                        marauder_prob = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
                    }
                }
                else if (associated_role != RoleType.ChaosRepressor)
                {
                    if (paired_entry.Value > 0)
                    {
                        repressor_prob = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
                    }
                }
                else if (associated_role != RoleType.ChaosRifleman)
                {
                    if (paired_entry.Value > 0)
                    {
                        rifleman_prob = (int)Math.Floor((float)playersToSpawn * paired_entry.Value);
                    }
                }
            }



            for (int marauder_counter = 0; marauder_counter < marauder_prob; marauder_counter++)
            {
                queueToFill.Enqueue(global::RoleType.ChaosMarauder);
                total_spawned += 1;
                if (total_spawned > playersToSpawn)
                {
                    return false;
                }
            }
            for (int repressor_counter = 0; repressor_counter < repressor_prob; repressor_counter++)
            {
                queueToFill.Enqueue(global::RoleType.ChaosRepressor);
                total_spawned += 1;
                if (total_spawned > playersToSpawn)
                {
                    return false;
                }
            }
            for (int rifleman_counter = 0; rifleman_counter < rifleman_prob; rifleman_counter++)
            {
                queueToFill.Enqueue(global::RoleType.ChaosRifleman);
                total_spawned += 1;
                if (total_spawned > playersToSpawn)
                {
                    return false;
                }
            }
            return false;
        }

        private bool AddNewPlayerToQueue(int amount_to_spawn, ref int total_spawned, RoleType role_to_spawn, int playersToSpawn, Queue<global::RoleType> queueToFill)
        {
            for (int chaos_to_spawn_counter = 0; chaos_to_spawn_counter < amount_to_spawn; chaos_to_spawn_counter++)
            {
                queueToFill.Enqueue(role_to_spawn);
                total_spawned += 1;
                if (total_spawned > playersToSpawn)
                {
                    return false;
                }
            }
            return true;
        }


    }
}
