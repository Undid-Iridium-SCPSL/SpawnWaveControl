using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace SpawnWaveControl
{
    public sealed class Config : IConfig
    {
        //public bool IsEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [Description("Whether to enable or disable plugin")]
        public bool IsEnabled { get; set; } = true;

        [Description("Rules of spawn percentage per Chaos type. Should add up to 1")]
        public Dictionary<string, float> ChaosSpawnWaveRules { get; set; } =
            new Dictionary<string, float>
            {
                { "ChaosMarauder" ,  0.2f },
                { "ChaosRepressor" ,  0.3f },
                { "ChaosRifleman" ,  1.0f}

            };

        [Description("Rules of spawn percentage per MTF type. Should add up to 1")]
        public Dictionary<string, float> MtfSpawnWaveRules { get; set; } =
            new Dictionary<string, float>
            {
                { "NtfCaptain" ,  .1f }, //1, <=0 will be ignored, and won't be a spawned type.
                { "NtfSergeant" ,  .2f }, //3
                { "NtfPrivate" ,  .7f } //Rest - 4

            };

        [Description("Rules of spawn percentage per Guard type (Not implemented yet?)")]
        public Dictionary<string, float> GuardSpawnWaveRules { get; set; } =
            new Dictionary<string, float>
            {
                { "ChaosMarauder" ,  0.2f },
                { "ChaosRepressor" ,  0.3f },
                { "ChaosRifleman" ,  1.0f}

            };

        [Description("Gives logic choice behavior based on what you set.")]
        public Dictionary<string, bool> ProgramLevel { get; set; } =
            new Dictionary<string, bool>
            {
                { "Chaos_Config", true },
                { "Mtf_Config", false },
                { "Gaurd_Config", false }
            };

        [Description("Debug flag.")]
        public bool debug_enabled { get; set; } = false;

    }
}
