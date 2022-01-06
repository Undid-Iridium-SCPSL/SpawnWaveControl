using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using Respawning;
using SpawnWaveControl.Patches;
using System;

namespace SpawnWaveControl
{
    public class SpawnWaveControl : Plugin<Config>
    {
        public static Config early_config;

        /// <summary>
        /// Medium priority, lower prioritys mean faster loadin
        /// </summary>
        public override PluginPriority Priority { get; } = PluginPriority.Higher;

        private Harmony harmony;
        private string harmony_id = "com.Undid-Iridium.SpawnControl";

        public override Version RequiredExiledVersion { get; } = new Version(4, 2, 0);

        /// <summary>
        /// Entrance function called through Exile
        /// </summary>
        public override void OnEnabled()
        {
            RegisterEvents();
            RegisterHarmony();
            base.OnEnabled();
        }

        /// <summary>
        /// Destruction function called through Exile
        /// </summary>
        public override void OnDisabled()
        {
            UnRegisterEvents();
            UnRegisterHarmony();
            base.OnDisabled();
        }



        private void RegisterHarmony()
        {
            harmony = new Harmony(harmony_id);
            //https://harmony.pardeike.net/articles/basics.html cute patching method
            if (Config.spawn_location_control)
            {
                harmony.PatchAll();
            }
            else
            {
                var chaos_original = typeof(ChaosInsurgencySpawnHandler).GetMethod(nameof(NineTailedFoxSpawnHandler.GenerateQueue));
                var chaos_prefix = typeof(ChaosSpawnPatcher).GetMethod(nameof(ChaosSpawnPatcher.ChaosSpawnPatch));
                harmony.Patch(chaos_original, prefix: new HarmonyMethod(chaos_prefix));

                var mtf_original = typeof(NineTailedFoxSpawnHandler).GetMethod(nameof(NineTailedFoxSpawnHandler.GenerateQueue));
                var mtf_prefix = typeof(MtfSpawnPatcher).GetMethod(nameof(MtfSpawnPatcher.MtfSpawnPatch));
                harmony.Patch(chaos_original, prefix: new HarmonyMethod(chaos_prefix));
            }
        }


        private void UnRegisterHarmony()
        {
            harmony.UnpatchAll(harmony.Id);
            harmony = null;
        }


        /// <summary>
        /// Registers events for EXILE to hook unto with cororotines (I think?)
        /// </summary>
        public void RegisterEvents()
        {
            // Register the event handler class. And add the event,
            // to the EXILED_Events event listener so we get the event.
            early_config = Config;

            Log.Info("SpawnControl has been loaded");

        }
        /// <summary>
        /// Unregisters the events defined in RegisterEvents, recommended that everything created be destroyed if not reused in some way.
        /// </summary>
        public void UnRegisterEvents()
        {
            Log.Info("SpawnControl has been unloaded");
        }
    }
}
