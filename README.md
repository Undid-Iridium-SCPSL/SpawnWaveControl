# SpawnWaveControl

![SPAWNWAVECONTROL LATEST](https://img.shields.io/github/v/release/Undid-Iridium/SpawnWaveControl?include_prereleases&style=flat-square)
![SPAWNWAVECONTROL LINES](https://img.shields.io/tokei/lines/github/Undid-Iridium/SpawnWaveControl)
![SPAWNWAVECONTROL DOWNLOADS](https://img.shields.io/github/downloads/Undid-Iridium/SpawnWaveControl/total?style=flat-square)


Ability to control spawn of MTF, CHAOS and location+

Solution Hijacks the GenerateSpawn configuration code that generates what waves will spawn. 

Solution Hijacks SpawnpointManager's code, and how it handles gathering of default locations

There is a bug with EXILED/NW where Chaos spawn may show the wrong hinttext of the type spawned, and f1 information is wrong

Nevertheless, the config is the following

Current Plugin Version: V1.0.4



# Installation

**[EXILED](https://github.com/galaxy119/EXILED) must be installed for this to work.**

Place the "SpawnWaveControl.dll" file in your Plugins folder.


## REQUIREMENTS
* Exiled: V4.2.0
* SCP:SL Server: V Latest game version 11.2

## Config
What roles can be spawned, and what their %change or %amount will be spawned. 
| Spawn Configuration              | Value Type | Description                                                                                                                                                  |
|----------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ChaosMarauder              | RoleType   | Determines the percent/probability of Marauder to spawn per team spawn. Chaos Insurgency                                                                           |
| ChaosRepressor             | RoleType   | Determines the percent/probability of Repressor to spawn per team spawn. Chaos Insurgency                                                                          |
| ChaosRifleman              | RoleType   | Determines the percent/probability of Rifleman to spawn per team spawn. Chaos Insurgency                                                                           |
| NtfCaptain                 | RoleType   | Determines the percent/probability of Captain to spawn per team spawn. Nine-Tailed Fox                                                                             |
| NtfSergeant                | RoleType   | Determines the percent/probability of Sergeant to spawn per team spawn. Nine-Tailed Fox                                                                            |
| NtfPrivate                 | RoleType   | Determines the percent/probability of Private to spawn per team spawn. Nine-Tailed Fox                                                                             |


| Configuration enable flags | Value Type | Description                                                                                                                                                        |
|----------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Chaos_Config               | bool       | Whether to enable override behavior for Chaos spawn rates. If not it defaults to base game logic                                                                   |
| Mtf_Config                 | bool       | Whether to enable override behavior for Mtf spawn rates. If not it defaults to base game logic                                                                     |


| Config probability flag    | Value Type | Description                                                                                                                                                        |
|----------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| probability_flag           | bool       | Whether to enable probability logic instead of percentage spawn.                                                                                                   |

Example of the config (SCP_939 obviously was for testing, putting this here because I have to run)
```
role_spawn_locations:
    FacilityGuard: SCP_939
    NtfPrivate: SCP_939
    NtfSergeant: SCP_939
    NtfCaptain: SCP_939
    NtfSpecialist: SCP_939
    ChaosConscript: SP_CI
    ChaosMarauder: SP_CI
    ChaosRepressor: SP_CI
    ChaosRifleman: SP_CI
    Scp049: SP_049
    Scp0492: 
    Scp079: SP_079
    Scp096: SCP_096
    Scp106: SP_106
    Scp173: SP_173
    Scp93953: SCP_939
    Scp93989: SCP_939
    ClassD: SP_CDP
    Scientist: SP_RSC
  # Gives spawn wave location control based on what you set. Would NOT recommend using this for MTF/SCP
  unique_groups_spawn_locations:
    MTF: SP_MTF
    CHI: SP_CI
    RSC: SP_RSC
    CDP: SP_CDP
    TUT: TUT Spawn
  # Flag to enable control of the spawn locations of specific groups
  spawn_location_control: true
```
