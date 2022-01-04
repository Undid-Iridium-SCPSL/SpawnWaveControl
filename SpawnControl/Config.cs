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

        [Description("Gives spawn wave location control based on what you set. Very graular control.")]
        public Dictionary<RoleType, string> RoleSpawnLocations { get; set; } =
            new Dictionary<RoleType, string>
            {
                { RoleType.FacilityGuard, "SP_GUARD"},
                { RoleType.NtfPrivate, "SP_MTF"},
                { RoleType.NtfSergeant, "SP_MTF"},
                { RoleType.NtfCaptain, "SP_MTF"},
                { RoleType.NtfSpecialist, "SP_MTF"},

                { RoleType.ChaosConscript,"SP_CI"},
                { RoleType.ChaosMarauder,"SP_CI"},
                { RoleType.ChaosRepressor,"SP_CI"},
                { RoleType.ChaosRifleman,"SP_CI"},

                { RoleType.Scp049, "SP_049"},
                { RoleType.Scp0492, null},
                { RoleType.Scp079, "SP_079"},
                { RoleType.Scp096, "SCP_096"},
                { RoleType.Scp106, "SP_106"},
                { RoleType.Scp173, "SP_173"},
                { RoleType.Scp93953, "SCP_939"},
                { RoleType.Scp93989, "SCP_939"}

            };

        [Description("Gives spawn wave location control based on what you set. Would NOT recommend using this for MTF/SCP")]
        public Dictionary<Team, string> UniqueGroupsSpawnLocations { get; set; } =
            new Dictionary<Team, string>
            {
                { Team.MTF, "SP_MTF" },
                { Team.CHI, "SP_CI" },
                { Team.RSC, "SP_RSC"},
                { Team.CDP, "SP_CDP" },
                { Team.TUT, "TUT Spawn" }
            };

        [Description("Flag to enable control of the spawn locations of specific groups")]
        public bool spawn_location_control { get; set; } = false;

        [Description("probability flag (Changes from % X will spawn to % chance to spawn X type).")]
        public bool probability_flag { get; set; } = false;


        [Description("Debug flag.")]
        public bool debug_enabled { get; set; } = false;

    }
}
