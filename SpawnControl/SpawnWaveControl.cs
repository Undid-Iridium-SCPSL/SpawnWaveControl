using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;

namespace SpawnWaveControl
{
    public class SpawnWaveControl : Plugin<Config>
    {
        public static Config early_config;

        /// <summary>
        /// Medium priority, lower prioritys mean faster loadin
        /// </summary>
        public override PluginPriority Priority { get; } = PluginPriority.Medium;





        /// <summary>
        /// Entrance function called through Exile
        /// </summary>
        public override void OnEnabled()
        {
            RegisterEvents();
            var harmony = new Harmony("com.Undid-Iridium.SpawnControl");
            harmony.PatchAll();

        }
        /// <summary>
        /// Destruction function called through Exile
        /// </summary>
        public override void OnDisabled()
        {
            UnRegisterEvents();
            var harmony = new Harmony("com.Undid-Iridium.SpawnControl");
            harmony.UnpatchAll("com.Undid-Iridium.SpawnControl");
        }



        /// <summary>
        /// Registers events for EXILE to hook unto with cororotines (I think?)
        /// </summary>
        public void RegisterEvents()
        {
            // Register the event handler class. And add the event,
            // to the EXILED_Events event listener so we get the event.
            if (!Config.IsEnabled)
            {
                return;
            }
            early_config = Config;


            //currentSpectator = new Handlers.SpectatorMonitor();

            //if (Config.ForceConstantUpdates)
            //{
            //    eventHandler = new Handlers.ForcedEventHandlers();
            //    PlayerEvents.ChangingRole += eventHandler.OnRoleChange;
            //}
            //else
            //{

            //    PlayerEvents.Died += currentSpectator.OnDeath;
            //    PlayerEvents.Spawning += currentSpectator.OnRespawn;
            //    PlayerEvents.ChangingRole += currentSpectator.OnChanginRole;

            //    ServerEvents.EndingRound += currentSpectator.OnRoundEnd;
            //    ServerEvents.RestartingRound += currentSpectator.OnRoundRestart;
            //    ServerEvents.WaitingForPlayers += currentSpectator.OnRoundRestart;
            //    ServerEvents.RespawningTeam += currentSpectator.OnTeamSpawn;
            //}


            Log.Info("SpawnControl has been loaded");

        }
        /// <summary>
        /// Unregisters the events defined in RegisterEvents, recommended that everything created be destroyed if not reused in some way.
        /// </summary>
        public void UnRegisterEvents()
        {
            // Make it dynamically updatable.
            // We do this by removing the listener for the event and then nulling the event handler.
            // This process must be repeated for each event.
            //if (Config.ForceConstantUpdates)
            //{
            //    eventHandler = null;
            //    PlayerEvents.ChangingRole -= eventHandler.OnRoleChange;
            //}
            //else
            //{
            //    PlayerEvents.Died -= currentSpectator.OnDeath;
            //    PlayerEvents.Spawning -= currentSpectator.OnRespawn;
            //    PlayerEvents.ChangingRole -= currentSpectator.OnChanginRole;

            //    ServerEvents.EndingRound -= currentSpectator.OnRoundEnd;
            //    ServerEvents.RestartingRound -= currentSpectator.OnRoundRestart;
            //    ServerEvents.WaitingForPlayers -= currentSpectator.OnRoundRestart;
            //    ServerEvents.RespawningTeam -= currentSpectator.OnTeamSpawn;
            //}
            //currentSpectator = null;

            Log.Info("SpawnControl has been unloaded");
        }
    }
}
