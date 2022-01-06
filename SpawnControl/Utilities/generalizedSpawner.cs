using System;
using System.Collections.Generic;

namespace SpawnWaveControl.Utilities
{
    public class generalizedSpawner
    {

        public static Random RandomGen = new Random();
        /// <summary>
        /// While thinking about whether I want to generalize the above approach, I am reminded of the story of ropes and users. 
        /// This is more of a proof of concept or a potential solution for later. 
        /// </summary>
        /// <param name="queueToFill"></param>
        /// <param name="playersToSpawn"></param>
        /// <param name="spawnType"></param>
        /// <param name="config_keys"></param>
        /// <returns></returns>
        public static bool GeneralizedSpawner(ref Queue<global::RoleType> queueToFill, int playersToSpawn, Dictionary<RoleType, float> config_keys, bool probability = false,
            char[] probability_arr = null, Dictionary<char, RoleType> associated_pair_key = null)
        {

            if (config_keys == null || config_keys.Count == 0)
            {
                //Runs the normal code execution
                return false;
            }


            //To prevent dumb configs. 
            int total_spawned = 0;

            RoleType last_known_role = RoleType.None;
            if (probability)
            {
                //This handles probability chance of an group being spawned instead of a percent that will be spawned. 
                LoggerTool.log_msg_static("Running probabilty logic!!");
                int current_random = 0;
                for (int pos = 0; pos < playersToSpawn; pos++)
                {
                    current_random = RandomGen.Next(0, 100);

                    RoleType role_to_spawn = associated_pair_key[probability_arr[current_random]];
                    LoggerTool.log_msg_static($"Current random {current_random} meaning {role_to_spawn} will spawn");
                    queueToFill.Enqueue(role_to_spawn);
                }
            }
            else
            {
                //This handles the default behavior of a % of the group being spawned as X type
                int players_to_queue = 0;
                foreach (KeyValuePair<RoleType, float> paired_spawn in config_keys)
                {
                    players_to_queue = (int)Math.Floor((float)playersToSpawn * paired_spawn.Value);
                    players_to_queue = players_to_queue < 1 ? 1 : players_to_queue;


                    last_known_role = paired_spawn.Key;
                    if (AddNewPlayerToQueue(players_to_queue, ref total_spawned, paired_spawn.Key, playersToSpawn, queueToFill))
                    {
                        LoggerTool.log_msg_static($"Alright, what was the type  { paired_spawn.Key} and its percentage {players_to_queue}");
                    }
                    else
                    {
                        LoggerTool.log_msg_static($"Alright, what was the type of non-added  { paired_spawn.Key} and its percentage {players_to_queue}");
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
