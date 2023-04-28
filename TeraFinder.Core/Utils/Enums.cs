namespace TeraFinder.Core;

public enum ScarletExclusives : ushort
{
    //TaurosBlazeBreed = 128,
    Larvitar = 246,
    Pupitar = 247,
    Tyranitar = 248,
    Drifloon = 425,
    Drifblim = 426,
    Stunky = 434,
    Skuntank = 435,
    Deino = 633,
    Zweilous = 634,
    Hydreigon = 635,
    Skrelp = 690,
    Dragalge = 691,
    Oranguru = 765,
    Stonjourner = 784,
    Armarouge = 936,
    GreatTusk = 984,
    ScreamTail = 985,
    BruteBonnet = 986,
    FlutterMane = 987,
    SlitherWing = 988,
    SandyShocks = 989,
    RoaringMoon = 1005,
    Koraidon = 1007,
    WalkingWake = 1009
}

public enum VioletExclusives : ushort
{
    //TaurosAquaBreed = 128,
    Misdreavus = 200,
    Gulpin = 316,
    Swalot = 317,
    Bagon = 371,
    Shelgon = 372,
    Salamence = 373,
    Mismagius = 429,
    Clauncher = 692,
    Clawitzer = 693,
    Passimian = 766,
    Dreepy = 885,
    Drakloak = 886,
    Dragapult = 887,
    Eiscue = 875,
    Ceruledge = 937,
    IronTreads = 990,
    IronBundle = 991,
    IronHands = 992,
    IronJugulis = 993,
    IronMoth = 994,
    IronThorns = 995,
    IronValiant = 1006,
    Miraidon = 1008,
    IronLeaves = 1010
}

public enum TeraShiny : int
{
    Any = 0,
    No = 1,
    Yes = 2,
    Star = 3,
    Square = 4,
}

public enum GameProgress : byte
{
    Beginning = 0,
    UnlockedTeraRaids = 1,
    Unlocked3Stars = 2,
    Unlocked4Stars = 3,
    Unlocked5Stars = 4,
    Unlocked6Stars = 5,
    None = 6,
}

public enum RaidContent : byte
{
    Standard = 0,
    Black = 1,
    Event = 2,
    Event_Mighty = 3,
}

public enum RewardCategory : int
{
    ItemNone = 0,
    Poke = 1,
    Gem = 2,
}

public enum RoutineType
{
    None,
    ReadWrite,
}
