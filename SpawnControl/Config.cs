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

        [Description("Rules of spawn percentage per Chaos type. Should add up to 1, if not logic will attempt to throttle to correct spawn limit")]
        public Dictionary<RoleType, float> ChaosSpawnWaveRules { get; set; } =
            new Dictionary<RoleType, float>
            {
                { RoleType.ChaosMarauder ,  0.2f },
                { RoleType.ChaosRepressor ,  0.3f },
                { RoleType.ChaosRifleman ,  0.7f}

            };

        [Description("Rules of spawn percentage per MTF type. Should add up to 1, if not logic will attempt to throttle to correct spawn limit")]
        public Dictionary<RoleType, float> MtfSpawnWaveRules { get; set; } =
            new Dictionary<RoleType, float>
            {
                { RoleType.NtfCaptain ,  0.1f }, //1, <=0 will be ignored, and won't be a spawned type.
                { RoleType.NtfSergeant ,  0.2f }, //3
                { RoleType.NtfPrivate ,  0.7f } //Rest - 4

            };

        [Description("Gives spawn wave behavior based on what you set.")]
        public Dictionary<string, bool> ProgramLevel { get; set; } =
            new Dictionary<string, bool>
            {
                { "Chaos_Config", true },
                { "Mtf_Config", false }
            };

        [Description("probability flag (Changes from % X will spawn to % chance to spawn X type).")]
        public bool probability_flag { get; set; } = false;


        [Description("Debug flag.")]
        public bool debug_enabled { get; set; } = false;

    }
}
