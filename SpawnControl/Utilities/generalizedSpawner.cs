using System;
using System.Collections.Generic;

namespace SpawnWaveControl.Utilities
{
    public class generalizedSpawner
    {

        /// <summary>
        /// While thinking about whether I want to generalize the above approach, I am reminded of the story of ropes and users. 
        /// This is more of a proof of concept or a potential solution for later. 
        /// </summary>
        /// <param name="queueToFill"></param>
        /// <param name="playersToSpawn"></param>
        /// <param name="spawnType"></param>
        /// <param name="config_keys"></param>
        /// <returns></returns>
        public static bool GeneralizedSpawner(ref Queue<global::RoleType> queueToFill, int playersToSpawn, Dictionary<string, float> config_keys)
        {

            if (config_keys == null || config_keys.Count == 0)
            {
                //Runs the normal code execution
                return false;
            }


            //To prevent dumb configs. 
            int total_spawned = 0;


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

            RoleType last_known_role = RoleType.None;
            foreach (KeyValuePair<RoleType, int> paired_spawn in role_probability_pair)
            {
                last_known_role = paired_spawn.Key;
                if (AddNewPlayerToQueue(paired_spawn.Value, ref total_spawned, paired_spawn.Key, playersToSpawn, queueToFill))
                {
                    LoggerTool.log_msg_static($"Alright, what was the probabilities  { paired_spawn.Key} and its prob {paired_spawn.Value}");
                }
                else
                {
                    LoggerTool.log_msg_static($"Alright, what was the probabilities of non-added  { paired_spawn.Key} and its prob {paired_spawn.Value}");
                }

            }


            //Incase the user somehow provided bad values, we should still either provide something to spawn if != players to spawn
            //Such as .2, .2, .2 which is .6, we need the other .4, so we will default to the last type defined. If
            //no known type is known, something went really bad and we will return and run default logic. 
            if (last_known_role != RoleType.None)
            {
                while (total_spawned < playersToSpawn)
                {
                    queueToFill.Enqueue(last_known_role);
                    total_spawned += 1;
                }
            }
            else
            {
                return false;
            }

            return true;
        }



        private static bool AddNewPlayerToQueue(int amount_to_spawn, ref int total_spawned, RoleType role_to_spawn, int playersToSpawn, Queue<global::RoleType> queueToFill)
        {

            for (int player_to_spawn_counter = 0; player_to_spawn_counter < amount_to_spawn; player_to_spawn_counter++)
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
