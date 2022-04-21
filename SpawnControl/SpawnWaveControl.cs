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


        private Harmony harmony;

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static SpawnWaveControl Instance { get; private set; }

        /// <inheritdoc />
        public override string Author => "Undid-Iridium";

        /// <inheritdoc />
        public override string Name => "SpawnWaveControl";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 3);

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 0, 5);





        /// <summary>
        /// Entrance function called through Exile
        /// </summary>
        public override void OnEnabled()
        {
            Instance = this;



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
            harmony = new Harmony($"com.Undid-Iridium.SpawnControl.{DateTime.UtcNow.Ticks}");
            //https://harmony.pardeike.net/articles/basics.html cute patching method
            if (SpawnWaveControl.Instance.Config.spawn_location_control)
            {
                harmony.PatchAll();
                return;
            }

            var chaos_original = typeof(ChaosInsurgencySpawnHandler).GetMethod(nameof(NineTailedFoxSpawnHandler.GenerateQueue));
            var chaos_prefix = typeof(ChaosSpawnPatcher).GetMethod(nameof(ChaosSpawnPatcher.ChaosSpawnPatch));
            harmony.Patch(chaos_original, prefix: new HarmonyMethod(chaos_prefix));

            var mtf_original = typeof(NineTailedFoxSpawnHandler).GetMethod(nameof(NineTailedFoxSpawnHandler.GenerateQueue));
            var mtf_prefix = typeof(MtfSpawnPatcher).GetMethod(nameof(MtfSpawnPatcher.MtfSpawnPatch));
            harmony.Patch(chaos_original, prefix: new HarmonyMethod(chaos_prefix));

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
