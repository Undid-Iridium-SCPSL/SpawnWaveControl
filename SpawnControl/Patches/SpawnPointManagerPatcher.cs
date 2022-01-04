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
            bool is_enabled = SpawnWaveControl.early_config.spawn_location_control;

            if (!is_enabled)
            {
                //Runs the normal code execution
                LoggerTool.log_msg_static("We were not enabled for SpawnPointPatcher. Letting original method run");
                return true;
            }

            Dictionary<RoleType, string> group_spawn_locations = SpawnWaveControl.early_config.RoleSpawnLocations;
            LoggerTool.log_msg_static("We were enabled and are now going to start touching things");
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

            LoggerTool.log_msg_static($"We are going to check if {unique_group_spawn_location == null} is null");
            if (unique_group_spawn_location == null)
            {
                LoggerTool.log_msg_static($"What is our classID {classID}");
                if (!group_spawn_location.TryGetValue(classID, out string unique_group_data))
                {

                    return default_nw_logic(classID);
                }
                else
                {
                    LoggerTool.log_msg_static($"What the hell is our classID unique group {unique_group_data}");
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
                            return default_nw_logic(classID);
                        }
                        return generate_new_scp_logic(classID, unique_team_data);
                    case global::Team.MTF:
                        if (!unique_group_spawn_location.TryGetValue(Team.MTF, out string mtf_unique_team_data))
                        {
                            return default_nw_logic(classID);
                        }
                        return GameObject.FindGameObjectsWithTag(mtf_unique_team_data);
                    case global::Team.CHI:
                        if (!unique_group_spawn_location.TryGetValue(Team.CHI, out string chi_unique_team_data))
                        {
                            return default_nw_logic(classID);
                        }
                        return GameObject.FindGameObjectsWithTag(chi_unique_team_data);
                    case global::Team.RSC:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string rsc_unique_team_data))
                        {
                            return default_nw_logic(classID);
                        }
                        return GameObject.FindGameObjectsWithTag(rsc_unique_team_data);
                    case global::Team.CDP:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string cdp_unique_team_data))
                        {
                            return default_nw_logic(classID);
                        }
                        return GameObject.FindGameObjectsWithTag(cdp_unique_team_data);
                    case global::Team.TUT:
                        if (!unique_group_spawn_location.TryGetValue(Team.SCP, out string tut_unique_team_data))
                        {
                            return default_nw_logic(classID);
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

        private static GameObject[] default_nw_logic(RoleType classID)
        {
            global::Role role = GameObject.Find("Host").GetComponent<global::CharacterClassManager>().Classes.SafeGet(classID);
            if (classID == global::RoleType.Scp0492)
            {
                return null;
            }
            switch (role.team)
            {
                case global::Team.SCP:
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
                case global::Team.MTF:
                    return GameObject.FindGameObjectsWithTag((classID == global::RoleType.FacilityGuard) ? "SP_GUARD" : "SP_MTF");
                case global::Team.CHI:
                    return GameObject.FindGameObjectsWithTag("SP_CI");
                case global::Team.RSC:
                    return GameObject.FindGameObjectsWithTag("SP_RSC");
                case global::Team.CDP:
                    return GameObject.FindGameObjectsWithTag("SP_CDP");
                case global::Team.TUT:
                    return new GameObject[]
                    {
                GameObject.Find("TUT Spawn")
                    };
            }
            return null;
        }
    }
}
