using HarmonyLib;
using SpawnWaveControl.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnWaveControl.Patches
{
    /*
 * [HarmonyPatch(typeof(PatchTargetClass), 
     nameof(PatchTargetClass.TargetMethodName), 
     new[] {typeof(float), typeof(int)} ]
*/

    //https://harmony.pardeike.net/articles/basics.html patch specific functions
    [HarmonyPatch(typeof(SpawnpointManager))]
    [HarmonyPatch(nameof(SpawnpointManager.FillSpawnPoints), MethodType.Normal)]
    public class SpawnPointManagerPatcher
    {

        [HarmonyPrefix]
        public static bool SpawnPointPatcher()
        {
            SpawnWaveControl.early_config.ProgramLevel.TryGetValue("spawn_location_control", out bool is_enabled);

            if (!is_enabled)
            {
                //Runs the normal code execution
                LoggerTool.log_msg_static("We were not enabled. Letting original method run");
                return true;
            }

            Dictionary<RoleType, string> group_spawn_locations = SpawnWaveControl.early_config.RoleSpawnLocations;

            global::SpawnpointManager.Positions.Clear();
            foreach (RoleType available_roles in Enum.GetValues(typeof(global::RoleType)))
            {
                if (available_roles != global::RoleType.None || available_roles != global::RoleType.Spectator)
                {
                    global::SpawnpointManager.Positions.Add(available_roles, GetArrayOfPositionsOverride(available_roles, group_spawn_locations));
                }
            }
            return false;
        }
        public static GameObject[] GetArrayOfPositionsOverride(global::RoleType classID,
            Dictionary<RoleType, string> group_spawn_location, Dictionary<Team, string> unique_group_spawn_location = null)
        {

            if (classID == global::RoleType.Scp0492)
            {
                return null;
            }

            if (unique_group_spawn_location == null)
            {
                if (!group_spawn_location.TryGetValue(classID, out string unique_group_data))
                {
                    return GameObject.FindGameObjectsWithTag("SP_RSC");
                }
                else
                {
                    return GameObject.FindGameObjectsWithTag(unique_group_data);
                }

            }
            else
            {
                global::Role role = GameObject.Find("Host").GetComponent<global::CharacterClassManager>().Classes.SafeGet(classID);
                switch (role.team)
                {
                    case global::Team.SCP:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string unique_team_data))
                        {
                            return default_scp_logic(classID);
                        }
                        return generate_new_scp_logic(classID, unique_team_data);
                    case global::Team.MTF:
                        if (!unique_group_spawn_location.TryGetValue(Team.MTF, out string mtf_unique_team_data))
                        {
                            return GameObject.FindGameObjectsWithTag((classID == global::RoleType.FacilityGuard) ? "SP_GUARD" : "SP_MTF");
                        }
                        return GameObject.FindGameObjectsWithTag(mtf_unique_team_data);
                    case global::Team.CHI:
                        if (!unique_group_spawn_location.TryGetValue(Team.CHI, out string chi_unique_team_data))
                        {
                            return GameObject.FindGameObjectsWithTag("SP_CI");
                        }
                        return GameObject.FindGameObjectsWithTag(chi_unique_team_data);
                    case global::Team.RSC:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string rsc_unique_team_data))
                        {
                            return GameObject.FindGameObjectsWithTag("SP_RSC");
                        }
                        return GameObject.FindGameObjectsWithTag(rsc_unique_team_data);
                    case global::Team.CDP:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string cdp_unique_team_data))
                        {
                            return GameObject.FindGameObjectsWithTag("SP_CDP");
                        }
                        return GameObject.FindGameObjectsWithTag(cdp_unique_team_data);
                    case global::Team.TUT:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string tut_unique_team_data))
                        {
                            return new GameObject[]
                            {
                                GameObject.Find("TUT Spawn")
                            };
                        }
                        return new GameObject[]
                        {
                            GameObject.Find(tut_unique_team_data)
                        };
                }
            }
            return null;
        }

        private static GameObject[] generate_new_scp_logic(RoleType classID, string unique_team_data)
        {
            switch (classID)
            {
                case global::RoleType.Scp106:
                    return GameObject.FindGameObjectsWithTag(unique_team_data);
                case global::RoleType.NtfSpecialist:
                case global::RoleType.Scientist:
                case global::RoleType.ChaosConscript:
                    break;
                case global::RoleType.Scp049:
                    return GameObject.FindGameObjectsWithTag(unique_team_data);
                case global::RoleType.Scp079:
                    return GameObject.FindGameObjectsWithTag(unique_team_data);
                case global::RoleType.Scp096:
                    return GameObject.FindGameObjectsWithTag(unique_team_data);
                default:
                    if (classID - global::RoleType.Scp93953 <= 1)
                    {
                        return GameObject.FindGameObjectsWithTag(unique_team_data);
                    }
                    break;
            }
            return GameObject.FindGameObjectsWithTag(unique_team_data);
        }

        private static GameObject[] default_scp_logic(RoleType classID)
        {
            switch (classID)
            {
                case global::RoleType.Scp106:
                    return GameObject.FindGameObjectsWithTag("SP_106");
                case global::RoleType.NtfSpecialist:
                case global::RoleType.Scientist:
                case global::RoleType.ChaosConscript:
                    break;
                case global::RoleType.Scp049:
                    return GameObject.FindGameObjectsWithTag("SP_049");
                case global::RoleType.Scp079:
                    return GameObject.FindGameObjectsWithTag("SP_079");
                case global::RoleType.Scp096:
                    return GameObject.FindGameObjectsWithTag("SCP_096");
                default:
                    if (classID - global::RoleType.Scp93953 <= 1)
                    {
                        return GameObject.FindGameObjectsWithTag("SCP_939");
                    }
                    break;
            }
            return GameObject.FindGameObjectsWithTag("SP_173");
        }
    }
}
