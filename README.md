# SpawnWaveControl

![SPAWNWAVECONTROL LATEST](https://img.shields.io/github/v/release/Undid-Iridium/SpawnWaveControl?include_prereleases&style=flat-square)
![SPAWNWAVECONTROL LINES](https://img.shields.io/tokei/lines/github/Undid-Iridium/SpawnWaveControl)
![SPAWNWAVECONTROL DOWNLOADS](https://img.shields.io/github/downloads/Undid-Iridium/SpawnWaveControl/total?style=flat-square)


Modify SpawnWave controls for MTF, and CI (NTF, CHAOS)+

Solution Hijacks the GenerateSpawn configuration code that generates what waves will spawn. 

There is a bug with EXILED/NW where Chaos spawn may show the wrong hinttext of the type spawned, and f1 information is wrong

Nevertheless, the config is the following

Current Plugin Version: V1.0.0

## REQUIREMENTS
* Exiled: V4.21.0
* SCP:SL Server: V11.0

## Config
What ghosts can interact with is not shown on this list but is configurable and will be generated along with the rest of these settings.
| Spawn Configuration              | Value Type | Description                                                                                                                                                  |
|----------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ChaosMarauder              | RoleType   | Determines the percent of Marauder to spawn per team spawn. Chaos Insurgency                                                                                       |
| ChaosRepressor             | RoleType   | Determines the percent of Repressor to spawn per team spawn. Chaos Insurgency                                                                                      |
| ChaosRifleman              | RoleType   | Determines the percent of Rifleman to spawn per team spawn. Chaos Insurgency                                                                                       |
| NtfCaptain                 | RoleType   | Determines the percent of Captain to spawn per team spawn. Chaos Insurgency                                                                                        |
| NtfSergeant                | RoleType   | Determines the percent of Sergeant to spawn per team spawn. Chaos Insurgency                                                                                       |
| NtfPrivate                 | RoleType   | Determines the percent of Private to spawn per team spawn. Chaos Insurgency                                                                                        |


| Configuration enable flags | Value Type | Description                                                                                                                                                        |
|----------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Chaos_Config               | bool       | Whether to enable override behavior for Chaos spawn rates. If not it defaults to base game logic                                                                   |
| Mtf_Config                 | bool       | Whether to enable override behavior for Mtf spawn rates. If not it defaults to base game logic                                                                     |


| Config probability flag    | Value Type | Description                                                                                                                                                        |
|----------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| probability_flag           | bool       | Whether to enable probability logic instead of percentage spawn.                                                                                                   |

