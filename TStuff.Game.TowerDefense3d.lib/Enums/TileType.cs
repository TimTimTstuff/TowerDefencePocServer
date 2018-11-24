namespace TStuff.Game.TowerDefense3d.lib.Enums
{
    public enum TileType
    {
        Block = 0x000000,
        NonBuild = 0xffffff,
        BuildTeam1 = 0xff0000,
        BuildTeam2 = 0x00ff00,
        BuildTeam3 = 0x0000ff,
        BuildTeam4 = 0xffff00,

        Path1Team2Start = 0x640000,
        Path1Team2Way = 0xaa0000,
        Path1Team2end = 0xee0000,

        Path2Team2Start = 0x630000,
        Path2Team2Way = 0xc10000,
        Path2Team2end = 0xe10000,

        Path3Team2Start = 0x620000,
        Path3Team2Way = 0xc20000,
        Path3Team2end = 0xe20000,

        Path4Team2Start = 0x610000,
        Path4Team2Way = 0xa30000,
        Path4Team2end = 0xe30000,

        Path1Team3Start = 0x006400,
        Path1Team3Way = 0x00aa00,
        Path1Team3end = 0x00ee00,

        Path2Team3Start = 0x006300,
        Path2Team3Way = 0x00a100,
        Path2Team3end = 0x00e100,

        Path3Team3Start = 0x006200,
        Path3Team3Way = 0x00a200,
        Path3Team3end = 0x00e200,

        Path4Team3Start = 0x006100,
        Path4Team3Way = 0x00a300,
        Path4Team3end = 0x00e300,

        Path1Team4Start = 0x000064,
        Path1Team4Way = 0x0000aa,
        Path1Team4end = 0x0000ee,

        Path2Team4Start = 0x000063,
        Path2Team4Way = 0x0000a1,
        Path2Team4end = 0x0000e1,

        Path3Team4Start = 0x000062,
        Path3Team4Way = 0x0000a2,
        Path3Team4end = 0x0000e2,

        Path4Team4Start = 0x000061,
        Path4Team4Way = 0x0000a3,
        Path4Team4end = 0x0000e3,

        Path1Team1Start = 0x646400,
        Path1Team1Way = 0xa1a100,
        Path1Team1end = 0xe1e100,

        Path2Team1Start = 0x636300,
        Path2Team1Way = 0xa2a200,
        Path2Team1end = 0xe2e200,

        Path3Team1Start = 0x626200,
        Path3Team1Way = 0xa3a300,
        Path3Team1end = 0xe3e300,

        Path4Team1Start = 0x616100,
        Path4Team1Way = 0xa4a400,
        Path4Team1end = 0xe4e400,
    }
}