using System.Text.Json;
using PKHeX.Core;

namespace TeraFinder
{

    public class Reward
    {
        public int ItemID { get; set; }
        public int Amount { get; set; }
        public int Probability { get; set; }
        public int Aux { get; set; }
    }

    public static class RewardUtil
    {
        public static readonly string[] TeraShard = new string[] { "Tera Shard", "テラピ", "Tera Shard", "Téra-Éclat", "Teralite", "Tera-Stück", "테라피스", "Teralito", "테라피스", "晶碎块", "晶碎塊" };
        public static readonly string[] Material = new string[] { "Material", "おとしもの", "Material", "Échantillons", "Materiali", "Materialentasche", "掉落物", "Materiales", "掉落物", "掉落物", "掉落物" };

        public static Dictionary<ulong, List<Reward>>[] GetTeraRewardsTables()
        {
            var drops = new RaidFixedRewardItemArray { Array = JsonSerializer.Deserialize<RaidFixedRewardItemArray.Root>(Properties.Resources.raid_fixed_reward_item_array)! };
            var lottery = new RaidLotteryRewardItemArray { Array = JsonSerializer.Deserialize<RaidLotteryRewardItemArray.Root>(Properties.Resources.raid_lottery_reward_item_array)! };
            var fixedTable = GetFixedTable(drops.Array.Table);
            var lotteryTable = GetLotteryTable(lottery.Array.Table);
            return new Dictionary<ulong, List<Reward>>[] { fixedTable, lotteryTable };
        }

        public static Dictionary<ulong, List<Reward>>[] GetDistRewardsTables(SAV9SV sav)
        {
            var rewards = EventUtil.GetEventItemDataFromSAV(sav);
            var drops = new RaidFixedRewardItemArray { Array = JsonSerializer.Deserialize<RaidFixedRewardItemArray.Root>(rewards[0])! };
            var lottery = new RaidLotteryRewardItemArray { Array = JsonSerializer.Deserialize<RaidLotteryRewardItemArray.Root>(rewards[1])! };
            var fixedTable = GetFixedTable(drops.Array.Table);
            var lotteryTable = GetLotteryTable(lottery.Array.Table);
            return new Dictionary<ulong, List<Reward>>[] { fixedTable, lotteryTable };
        }

        private static int GetTeraShard(MoveType type)
        {
            return type switch
            {
                MoveType.Normal => 1862,
                MoveType.Fighting => 1868,
                MoveType.Flying => 1871,
                MoveType.Poison => 1869,
                MoveType.Ground => 1870,
                MoveType.Rock => 1874,
                MoveType.Bug => 1873,
                MoveType.Ghost => 1875,
                MoveType.Steel => 1878,
                MoveType.Fire => 1863,
                MoveType.Water => 1864,
                MoveType.Grass => 1866,
                MoveType.Electric => 1865,
                MoveType.Psychic => 1872,
                MoveType.Ice => 1867,
                MoveType.Dragon => 1876,
                MoveType.Dark => 1877,
                MoveType.Fairy => 1879,
                _ => ushort.MaxValue - 1,
            };
        }

        private static int GetMaterial(Species species)
        {
            return species switch
            {
                Species.Venonat or Species.Venomoth => 1956,
                Species.Diglett or Species.Dugtrio => 1957,
                Species.Meowth or Species.Persian => 1958,
                Species.Psyduck or Species.Golduck => 1959,
                Species.Mankey or Species.Primeape or Species.Annihilape => 1960,
                Species.Growlithe or Species.Arcanine => 1961,
                Species.Slowpoke or Species.Slowbro or Species.Slowking => 1962,
                Species.Magnemite or Species.Magneton or Species.Magnezone => 1963,
                Species.Grimer or Species.Muk => 1964,
                Species.Shellder or Species.Cloyster => 1965,
                Species.Gastly or Species.Haunter or Species.Gengar => 1966,
                Species.Drowzee or Species.Hypno => 1967,
                Species.Voltorb or Species.Electrode => 1968,
                Species.Scyther or Species.Scizor or Species.Kleavor => 1969,
                Species.Tauros => 1970,
                Species.Magikarp or Species.Gyarados => 1971,
                Species.Ditto => 1972,
                Species.Eevee or Species.Vaporeon or Species.Jolteon
                or Species.Flareon or Species.Espeon or Species.Umbreon
                or Species.Leafeon or Species.Glaceon or Species.Sylveon => 1973,
                Species.Dratini or Species.Dragonair or Species.Dragonite => 1974,
                Species.Pichu or Species.Pikachu or Species.Raichu => 1975,
                Species.Igglybuff or Species.Jigglypuff or Species.Wigglytuff => 1976,
                Species.Mareep or Species.Flaaffy or Species.Ampharos => 1977,
                Species.Hoppip or Species.Skiploom or Species.Jumpluff => 1978,
                Species.Sunkern or Species.Sunflora => 1979,
                Species.Murkrow or Species.Honchkrow => 1980,
                Species.Misdreavus or Species.Mismagius => 1981,
                Species.Girafarig or Species.Farigiraf => 1982,
                Species.Pineco or Species.Forretress => 1983,
                Species.Dunsparce or Species.Dudunsparce => 1984,
                Species.Qwilfish or Species.Overqwil => 1985,
                Species.Heracross => 1986,
                Species.Sneasel or Species.Weavile or Species.Sneasler => 1987,
                Species.Teddiursa or Species.Ursaring or Species.Ursaluna => 1988,
                Species.Delibird => 1989,
                Species.Houndour or Species.Houndoom => 1990,
                Species.Phanpy or Species.Donphan => 1991,
                Species.Stantler or Species.Wyrdeer => 1992,
                Species.Larvitar or Species.Pupitar or Species.Tyranitar => 1994,
                Species.Wingull or Species.Pelipper => 1994,
                Species.Ralts or Species.Kirlia or Species.Gardevoir or Species.Gallade => 1995,
                Species.Surskit or Species.Masquerain => 1996,
                Species.Shroomish or Species.Breloom => 1997,
                Species.Slakoth or Species.Vigoroth or Species.Slaking => 1998,
                Species.Makuhita or Species.Hariyama => 1999,
                Species.Azurill or Species.Marill or Species.Azumarill => 2000,
                Species.Sableye => 2001,
                Species.Meditite or Species.Medicham => 2002,
                Species.Gulpin or Species.Swalot => 2003,
                Species.Numel or Species.Camerupt => 2004,
                Species.Torkoal => 2005,
                Species.Spoink or Species.Grumpig => 2006,
                Species.Cacnea or Species.Cacturne => 2007,
                Species.Swablu or Species.Altaria => 2008,
                Species.Zangoose => 2009,
                Species.Seviper => 2010,
                Species.Barboach or Species.Whiscash => 2011,
                Species.Shuppet or Species.Banette => 2012,
                Species.Tropius => 2013,
                Species.Snorunt or Species.Glalie or Species.Froslass => 2014,
                Species.Luvdisc => 2015,
                Species.Bagon or Species.Shelgon or Species.Salamence => 2016,
                Species.Starly or Species.Staravia or Species.Staraptor => 2017,
                Species.Kricketot or Species.Kricketune => 2018,
                Species.Shinx or Species.Luxio or Species.Luxray => 2019,
                Species.Combee or Species.Vespiquen => 2020,
                Species.Pachirisu => 2021,
                Species.Buizel or Species.Floatzel => 2022,
                Species.Shellos or Species.Gastrodon => 2023,
                Species.Drifloon or Species.Drifblim => 2024,
                Species.Stunky or Species.Skuntank => 2025,
                Species.Bronzor or Species.Bronzong => 2026,
                Species.Bonsly or Species.Sudowoodo => 2027,
                Species.Happiny or Species.Chansey or Species.Blissey => 2028,
                Species.Spiritomb => 2029,
                Species.Gible or Species.Gabite or Species.Garchomp => 2030,
                Species.Riolu or Species.Lucario => 2031,
                Species.Hippopotas or Species.Hippowdon => 2032,
                Species.Croagunk or Species.Toxicroak => 2033,
                Species.Finneon or Species.Lumineon => 2034,
                Species.Snover or Species.Abomasnow => 2035,
                Species.Rotom => 2036,
                Species.Petilil or Species.Lilligant => 2037,
                Species.Basculin or Species.Basculegion => 2038,
                Species.Sandile or Species.Krokorok or Species.Krookodile => 2039,
                Species.Zorua or Species.Zoroark => 2040,
                Species.Gothita or Species.Gothorita or Species.Gothitelle => 2041,
                Species.Deerling or Species.Sawsbuck => 2042,
                Species.Foongus or Species.Amoonguss => 2043,
                Species.Alomomola => 2044,
                Species.Tynamo or Species.Eelektrik or Species.Eelektross => 2045,
                Species.Axew or Species.Fraxure or Species.Haxorus => 2046,
                Species.Cubchoo or Species.Beartic => 2047,
                Species.Cryogonal => 2048,
                Species.Pawniard or Species.Bisharp or Species.Kingambit => 2049,
                Species.Rufflet or Species.Braviary => 2050,
                Species.Deino or Species.Zweilous or Species.Hydreigon => 2051,
                Species.Larvesta or Species.Volcarona => 2052,
                Species.Fletchling or Species.Fletchinder or Species.Talonflame => 2053,
                Species.Scatterbug or Species.Spewpa or Species.Vivillon => 2054,
                Species.Litleo or Species.Pyroar => 2055,
                Species.Flabébé or Species.Floette or Species.Florges => 2056,
                Species.Skiddo or Species.Gogoat => 2057,
                Species.Skrelp or Species.Dragalge => 2058,
                Species.Clauncher or Species.Clawitzer => 2059,
                Species.Hawlucha => 2060,
                Species.Dedenne => 2061,
                Species.Goomy or Species.Sliggoo or Species.Goodra => 2062,
                Species.Klefki => 2063,
                Species.Bergmite or Species.Avalugg => 2064,
                Species.Noibat or Species.Noivern => 2065,
                Species.Yungoos or Species.Gumshoos => 2066,
                Species.Crabrawler or Species.Crabominable => 2067,
                Species.Oricorio => 2068,
                Species.Rockruff or Species.Lycanroc => 2069,
                Species.Mareanie or Species.Toxapex => 2070,
                Species.Mudbray or Species.Mudsdale => 2071,
                Species.Fomantis or Species.Lurantis => 2072,
                Species.Salandit or Species.Salazzle => 2073,
                Species.Bounsweet or Species.Steenee or Species.Tsareena => 2074,
                Species.Oranguru => 2075,
                Species.Passimian => 2076,
                Species.Sandygast or Species.Palossand => 2077,
                Species.Komala => 2078,
                Species.Mimikyu => 2079,
                Species.Bruxish => 2080,
                Species.Chewtle or Species.Drednaw => 2081,
                Species.Skwovet or Species.Greedent => 2082,
                Species.Arrokuda or Species.Barraskewda => 2083,
                Species.Rookidee or Species.Corvisquire or Species.Corviknight => 2084,
                Species.Toxel or Species.Toxtricity => 2085,
                Species.Falinks => 2086,
                Species.Cufant or Species.Copperajah => 2087,
                Species.Rolycoly or Species.Carkol or Species.Coalossal => 2088,
                Species.Silicobra or Species.Sandaconda => 2089,
                Species.Indeedee => 2090,
                Species.Pincurchin => 2091,
                Species.Snom or Species.Frosmoth => 2092,
                Species.Impidimp or Species.Morgrem or Species.Grimmsnarl => 2093,
                Species.Applin or Species.Flapple or Species.Appletun => 2094,
                Species.Sinistea or Species.Polteageist => 2095,
                Species.Hatenna or Species.Hattrem or Species.Hatterene => 2096,
                Species.Stonjourner => 2097,
                Species.Eiscue => 2098,
                Species.Dreepy or Species.Drakloak or Species.Dragapult => 2099,

                Species.Lechonk or Species.Oinkologne => 2103,
                Species.Tarountula or Species.Spidops => 2104,
                Species.Nymble or Species.Lokix => 2105,
                Species.Rellor or Species.Rabsca => 2106,
                Species.Greavard or Species.Houndstone => 2107,
                Species.Flittle or Species.Espathra => 2108,
                Species.Wiglett or Species.Wugtrio => 2109,
                Species.Dondozo => 2110,
                Species.Veluza => 2111,
                Species.Finizen or Species.Palafin => 2112,
                Species.Smoliv or Species.Dolliv or Species.Arboliva => 2113,
                Species.Capsakid or Species.Scovillain => 2114,
                Species.Tadbulb or Species.Bellibolt => 2115,
                Species.Varoom or Species.Revavroom => 2116,
                Species.Orthworm => 2117,
                Species.Tandemaus or Species.Maushold => 2118,
                Species.Cetoddle or Species.Cetitan => 2119,
                Species.Frigibax or Species.Arctibax or Species.Baxcalibur => 2120,
                Species.Tatsugiri => 2121,
                Species.Cyclizar => 2122,
                Species.Pawmi or Species.Pawmo or Species.Pawmot => 2123,

                Species.Wattrel or Species.Kilowattrel => 2126,
                Species.Bombirdier => 2127,
                Species.Squawkabilly => 2128,
                Species.Flamigo => 2129,
                Species.Klawf => 2130,
                Species.Nacli or Species.Naclstack or Species.Garganacl => 2131,
                Species.Glimmet or Species.Glimmora => 2132,
                Species.Shroodle or Species.Grafaiai => 2133,
                Species.Fidough or Species.Dachsbun => 2134,
                Species.Maschiff or Species.Mabosstiff => 2135,
                Species.Bramblin or Species.Brambleghast => 2136,
                Species.Gimmighoul or Species.Gholdengo => 2137,

                Species.Tinkatink or Species.Tinkatuff or Species.Tinkaton => 2156,
                Species.Charcadet or Species.Armarouge or Species.Ceruledge => 2157,
                Species.Toedscool or Species.Toedscruel => 2158,
                Species.Wooper or Species.Quagsire or Species.Clodsire => 2159,

                _ => ushort.MaxValue,
            };
        }


        //Slightly modified from https://github.com/LegoFigure11/RaidCrawler/blob/06a7f4c17fca74297d6199f37a171f2b480d40f0/Structures/RaidRewards.cs#L10
        //GPL v3 License
        //Thanks LegoFigure11 & Architdate!
        public static List<Reward> GetRewardList(TeraDetails pkm, ulong fixedhash, ulong lotteryhash, Dictionary<ulong, List<Reward>>? fixedic = null, Dictionary<ulong, List<Reward>>? lotterydic = null, int boost = 0)
        {
            var rewardlist = new List<Reward>();
            var fixedlist = new List<Reward>();
            var lotterylist = new List<Reward>();

            var fixedexists = fixedic is not null && fixedic.TryGetValue(fixedhash, out fixedlist);
            var lotteryexists = lotterydic is not null && lotterydic.TryGetValue(lotteryhash, out lotterylist);

            if (fixedexists)
            {
                foreach (var reward in fixedlist!)
                {
                    rewardlist.Add(reward.ItemID == ushort.MaxValue ? new Reward { ItemID = GetMaterial(pkm.Species), Amount = reward.Amount, Aux = reward.Aux } :
                        reward.ItemID == ushort.MaxValue - 1 ? new Reward { ItemID = GetTeraShard(pkm.TeraType), Amount = reward.Amount, Aux = reward.Aux } : reward);
                }
            }
            if (lotteryexists)
            {
                var xoro = new Xoroshiro128Plus(pkm.Seed);
                var amount = GetRewardCount(xoro.NextInt(100), pkm.Stars) + boost;
                for (var i = 0; i < amount; i++)
                {
                    var treshold = (int)xoro.NextInt((ulong)lotterylist!.ElementAt(0).Aux);
                    foreach (var reward in lotterylist!)
                    {
                        if (reward.Probability > treshold)
                        {
                            rewardlist.Add(reward.ItemID == ushort.MaxValue ? new Reward { ItemID = GetMaterial(pkm.Species), Amount = reward.Amount } :
                                reward.ItemID == ushort.MaxValue - 1 ? new Reward { ItemID = GetTeraShard(pkm.TeraType), Amount = reward.Amount } : reward);
                            break;
                        }
                        treshold -= reward.Probability;
                    }
                }
            }
            return rewardlist;
        }

        public static List<Reward> GetRewardList(uint seed, int stars, ulong fixedhash, ulong lotteryhash, Dictionary<ulong, List<Reward>>? fixedic = null, Dictionary<ulong, List<Reward>>? lotterydic = null, int boost = 0)
        {
            var rewardlist = new List<Reward>();
            var fixedlist = new List<Reward>();
            var lotterylist = new List<Reward>();

            var fixedexists = fixedic is not null && fixedic.TryGetValue(fixedhash, out fixedlist);
            var lotteryexists = lotterydic is not null && lotterydic.TryGetValue(lotteryhash, out lotterylist);

            if (fixedexists)
            {
                rewardlist.AddRange(fixedlist!);
            }
            if (lotteryexists)
            {
                var xoro = new Xoroshiro128Plus(seed);
                var amount = GetRewardCount(xoro.NextInt(100), stars) + boost;
                for (var i = 0; i < amount; i++)
                {
                    var tres = (int)xoro.NextInt((ulong)lotterylist!.ElementAt(0).Aux);
                    foreach (var reward in lotterylist!)
                    {
                        if (reward.Probability > tres)
                        {
                            rewardlist.Add(reward);
                            break;
                        }
                        tres -= reward.Probability;
                    }
                }
            }
            return rewardlist;
        }

        private static readonly int[][] RewardSlots =
{
            new [] { 4, 5, 6, 7, 8 },
            new [] { 4, 5, 6, 7, 8 },
            new [] { 5, 6, 7, 8, 9 },
            new [] { 5, 6, 7, 8, 9 },
            new [] { 6, 7, 8, 9, 10 },
            new [] { 7, 8, 9, 10, 11 },
            new [] { 7, 8, 9, 10, 11 },
        };

        private static int GetRewardCount(ulong random, int stars)
        {
            return random switch
            {
                < 10 => RewardSlots[stars - 1][0],
                < 40 => RewardSlots[stars - 1][1],
                < 70 => RewardSlots[stars - 1][2],
                < 90 => RewardSlots[stars - 1][3],
                _ => RewardSlots[stars - 1][4],
            };
        }


        //Port from https://github.com/SteveCookTU/sv_raid_reader/blob/master/src/item_list.rs
        //Thanks SteveCookTU/EzPzStreamz!
        private static Dictionary<ulong, List<Reward>> GetFixedTable(List<RaidFixedRewardItemArray.Table> drops)
        {
            var table = new Dictionary<ulong, List<Reward>>();
            foreach (var d in drops)
            {
                var hash = d.TableName;

                if (table.ContainsKey(hash))
                    continue;

                var items = new List<Reward>();

                //RewardItem00
                if (d.RewardItem00.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem00.ItemID,
                        Amount = d.RewardItem00.Num,
                        Probability = 100,
                        Aux = d.RewardItem00.SubjectType,
                    });
                else if (d.RewardItem00.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem00.Num,
                        Probability = 100,
                        Aux = d.RewardItem00.SubjectType,
                    });
                }
                else if (d.RewardItem00.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem00.Num,
                        Probability = 100,
                        Aux = d.RewardItem00.SubjectType,
                    });
                }

                //RewardItem01
                if (d.RewardItem01.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem01.ItemID,
                        Amount = d.RewardItem01.Num,
                        Probability = 100,
                        Aux = d.RewardItem01.SubjectType,
                    });
                else if (d.RewardItem01.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem01.Num,
                        Probability = 100,
                        Aux = d.RewardItem01.SubjectType,
                    });
                }
                else if (d.RewardItem01.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem01.Num,
                        Probability = 100,
                        Aux = d.RewardItem01.SubjectType,
                    });
                }

                //RewardItem02
                if (d.RewardItem02.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem02.ItemID,
                        Amount = d.RewardItem02.Num,
                        Probability = 100,
                        Aux = d.RewardItem02.SubjectType,
                    });
                else if (d.RewardItem02.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem02.Num,
                        Probability = 100,
                        Aux = d.RewardItem02.SubjectType,
                    });
                }
                else if (d.RewardItem02.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem02.Num,
                        Probability = 100,
                        Aux = d.RewardItem02.SubjectType,
                    });
                }

                //RewardItem03
                if (d.RewardItem03.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem03.ItemID,
                        Amount = d.RewardItem03.Num,
                        Probability = 100,
                        Aux = d.RewardItem03.SubjectType,
                    });
                else if (d.RewardItem03.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem03.Num,
                        Probability = 100,
                        Aux = d.RewardItem03.SubjectType,
                    });
                }
                else if (d.RewardItem03.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem03.Num,
                        Probability = 100,
                        Aux = d.RewardItem03.SubjectType,
                    });
                }

                //RewardItem04
                if (d.RewardItem04.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem04.ItemID,
                        Amount = d.RewardItem04.Num,
                        Probability = 100,
                        Aux = d.RewardItem04.SubjectType,
                    });
                else if (d.RewardItem04.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem04.Num,
                        Probability = 100,
                        Aux = d.RewardItem04.SubjectType,
                    });
                }
                else if (d.RewardItem04.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem04.Num,
                        Probability = 100,
                        Aux = d.RewardItem04.SubjectType,
                    });
                }

                //RewardItem05
                if (d.RewardItem05.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem05.ItemID,
                        Amount = d.RewardItem05.Num,
                        Probability = 100,
                        Aux = d.RewardItem05.SubjectType,
                    });
                else if (d.RewardItem05.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem05.Num,
                        Probability = 100,
                        Aux = d.RewardItem05.SubjectType,
                    });
                }
                else if (d.RewardItem05.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem05.Num,
                        Probability = 100,
                        Aux = d.RewardItem05.SubjectType,
                    });
                }

                //RewardItem06
                if (d.RewardItem06.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem06.ItemID,
                        Amount = d.RewardItem06.Num,
                        Probability = 100,
                        Aux = d.RewardItem06.SubjectType,
                    });
                else if (d.RewardItem06.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem06.Num,
                        Probability = 100,
                        Aux = d.RewardItem06.SubjectType,
                    });
                }
                else if (d.RewardItem06.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem06.Num,
                        Probability = 100,
                        Aux = d.RewardItem06.SubjectType,
                    });
                }

                //RewardItem07
                if (d.RewardItem07.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem07.ItemID,
                        Amount = d.RewardItem07.Num,
                        Probability = 100,
                        Aux = d.RewardItem07.SubjectType,
                    });
                else if (d.RewardItem07.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem07.Num,
                        Probability = 100,
                        Aux = d.RewardItem07.SubjectType,
                    });
                }
                else if (d.RewardItem07.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem07.Num,
                        Probability = 100,
                        Aux = d.RewardItem07.SubjectType,
                    });
                }

                //RewardItem08
                if (d.RewardItem08.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem08.ItemID,
                        Amount = d.RewardItem08.Num,
                        Probability = 100,
                        Aux = d.RewardItem08.SubjectType,
                    });
                else if (d.RewardItem08.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem08.Num,
                        Probability = 100,
                        Aux = d.RewardItem08.SubjectType,
                    });
                }
                else if (d.RewardItem08.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem08.Num,
                        Probability = 100,
                        Aux = d.RewardItem08.SubjectType,
                    });
                }

                //RewardItem09
                if (d.RewardItem09.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem09.ItemID,
                        Amount = d.RewardItem09.Num,
                        Probability = 100,
                        Aux = d.RewardItem09.SubjectType,
                    });
                else if (d.RewardItem09.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem09.Num,
                        Probability = 100,
                        Aux = d.RewardItem09.SubjectType,
                    });
                }
                else if (d.RewardItem09.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem09.Num,
                        Probability = 100,
                        Aux = d.RewardItem09.SubjectType,
                    });
                }

                //RewardItem10
                if (d.RewardItem10.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem10.ItemID,
                        Amount = d.RewardItem10.Num,
                        Probability = 100,
                        Aux = d.RewardItem10.SubjectType,
                    });
                else if (d.RewardItem10.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem10.Num,
                        Probability = 100,
                        Aux = d.RewardItem10.SubjectType,
                    });
                }
                else if (d.RewardItem10.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem10.Num,
                        Probability = 100,
                        Aux = d.RewardItem10.SubjectType,
                    });
                }

                //RewardItem11
                if (d.RewardItem11.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem11.ItemID,
                        Amount = d.RewardItem11.Num,
                        Probability = 100,
                        Aux = d.RewardItem11.SubjectType,
                    });
                else if (d.RewardItem11.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem11.Num,
                        Probability = 100,
                        Aux = d.RewardItem11.SubjectType,
                    });
                }
                else if (d.RewardItem11.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem11.Num,
                        Probability = 100,
                        Aux = d.RewardItem11.SubjectType,
                    });
                }

                //RewardItem12
                if (d.RewardItem12.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem12.ItemID,
                        Amount = d.RewardItem12.Num,
                        Probability = 100,
                        Aux = d.RewardItem12.SubjectType,
                    });
                else if (d.RewardItem12.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem12.Num,
                        Probability = 100,
                        Aux = d.RewardItem12.SubjectType,
                    });
                }
                else if (d.RewardItem12.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem12.Num,
                        Probability = 100,
                        Aux = d.RewardItem12.SubjectType,
                    });
                }

                //RewardItem13
                if (d.RewardItem13.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem13.ItemID,
                        Amount = d.RewardItem13.Num,
                        Probability = 100,
                        Aux = d.RewardItem13.SubjectType,
                    });
                else if (d.RewardItem13.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem13.Num,
                        Probability = 100,
                        Aux = d.RewardItem13.SubjectType,
                    });
                }
                else if (d.RewardItem13.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem13.Num,
                        Probability = 100,
                        Aux = d.RewardItem13.SubjectType,
                    });
                }

                //RewardItem14
                if (d.RewardItem14.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem14.ItemID,
                        Amount = d.RewardItem14.Num,
                        Probability = 100,
                        Aux = d.RewardItem14.SubjectType,
                    });
                else if (d.RewardItem14.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem14.Num,
                        Probability = 100,
                        Aux = d.RewardItem14.SubjectType,
                    });
                }
                else if (d.RewardItem14.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem14.Num,
                        Probability = 100,
                        Aux = d.RewardItem14.SubjectType,
                    });
                }
                if (items.Count > 0)
                    table.Add(hash, items);
            }
            return table;
        }

        private static Dictionary<ulong, List<Reward>> GetLotteryTable(List<RaidLotteryRewardItemArray.Table> lottery)
        {
            var table = new Dictionary<ulong, List<Reward>>();

            foreach (var l in lottery)
            {
                var hash = l.TableName;

                if (table.ContainsKey(hash))
                    continue;

                var items = new List<Reward>();

                var rate = 0;
                rate += l.RewardItem00.Rate;
                rate += l.RewardItem01.Rate;
                rate += l.RewardItem02.Rate;
                rate += l.RewardItem03.Rate;
                rate += l.RewardItem04.Rate;
                rate += l.RewardItem05.Rate;
                rate += l.RewardItem06.Rate;
                rate += l.RewardItem07.Rate;
                rate += l.RewardItem08.Rate;
                rate += l.RewardItem09.Rate;
                rate += l.RewardItem10.Rate;
                rate += l.RewardItem11.Rate;
                rate += l.RewardItem12.Rate;
                rate += l.RewardItem13.Rate;
                rate += l.RewardItem14.Rate;
                rate += l.RewardItem15.Rate;
                rate += l.RewardItem16.Rate;
                rate += l.RewardItem17.Rate;
                rate += l.RewardItem18.Rate;
                rate += l.RewardItem19.Rate;
                rate += l.RewardItem20.Rate;
                rate += l.RewardItem21.Rate;
                rate += l.RewardItem22.Rate;
                rate += l.RewardItem23.Rate;
                rate += l.RewardItem24.Rate;
                rate += l.RewardItem25.Rate;
                rate += l.RewardItem26.Rate;
                rate += l.RewardItem27.Rate;
                rate += l.RewardItem28.Rate;
                rate += l.RewardItem29.Rate;

                var totalrate = rate;

                //RewardItem00
                if (l.RewardItem00.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem00.ItemID,
                        Amount = l.RewardItem00.Num,
                        Probability = l.RewardItem00.Rate,
                    });
                }
                else if (l.RewardItem00.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem00.Num,
                        Probability = l.RewardItem00.Rate
                    });
                }
                else if (l.RewardItem00.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem00.Num,
                        Probability = l.RewardItem00.Rate,
                    });
                }

                //RewardItem01
                if (l.RewardItem01.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem01.ItemID,
                        Amount = l.RewardItem01.Num,
                        Probability = l.RewardItem01.Rate
                    });
                }
                else if (l.RewardItem01.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem01.Num,
                        Probability = l.RewardItem01.Rate
                    });
                }
                else if (l.RewardItem01.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem01.Num,
                        Probability = l.RewardItem01.Rate,
                    });
                }

                //RewardItem02
                if (l.RewardItem02.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem02.ItemID,
                        Amount = l.RewardItem02.Num,
                        Probability = l.RewardItem02.Rate
                    });
                }
                else if (l.RewardItem02.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem02.Num,
                        Probability = l.RewardItem02.Rate
                    });
                }
                else if (l.RewardItem02.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem02.Num,
                        Probability = l.RewardItem02.Rate,
                    });
                }

                //RewardItem03
                if (l.RewardItem03.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem03.ItemID,
                        Amount = l.RewardItem03.Num,
                        Probability = l.RewardItem03.Rate
                    });
                }
                else if (l.RewardItem03.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem03.Num,
                        Probability = l.RewardItem03.Rate
                    });
                }
                else if (l.RewardItem03.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem03.Num,
                        Probability = l.RewardItem03.Rate,
                    });
                }

                //RewardItem04
                if (l.RewardItem04.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem04.ItemID,
                        Amount = l.RewardItem04.Num,
                        Probability = l.RewardItem04.Rate
                    });
                }
                else if (l.RewardItem04.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem04.Num,
                        Probability = l.RewardItem04.Rate
                    });
                }
                else if (l.RewardItem04.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem04.Num,
                        Probability = l.RewardItem04.Rate,
                    });
                }

                //RewardItem05
                if (l.RewardItem05.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem05.ItemID,
                        Amount = l.RewardItem05.Num,
                        Probability = l.RewardItem05.Rate
                    });
                }
                else if (l.RewardItem05.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem05.Num,
                        Probability = l.RewardItem05.Rate
                    });
                }
                else if (l.RewardItem05.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem05.Num,
                        Probability = l.RewardItem05.Rate,
                    });
                }

                //RewardItem06
                if (l.RewardItem06.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem06.ItemID,
                        Amount = l.RewardItem06.Num,
                        Probability = l.RewardItem06.Rate
                    });
                }
                else if (l.RewardItem06.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem06.Num,
                        Probability = l.RewardItem06.Rate
                    });
                }
                else if (l.RewardItem06.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem06.Num,
                        Probability = l.RewardItem06.Rate,
                    });
                }

                //RewardItem07
                if (l.RewardItem07.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem07.ItemID,
                        Amount = l.RewardItem07.Num,
                        Probability = l.RewardItem07.Rate
                    });
                }
                else if (l.RewardItem07.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem07.Num,
                        Probability = l.RewardItem07.Rate
                    });
                }
                else if (l.RewardItem07.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem07.Num,
                        Probability = l.RewardItem07.Rate,
                    });
                }

                //RewardItem08
                if (l.RewardItem08.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem08.ItemID,
                        Amount = l.RewardItem08.Num,
                        Probability = l.RewardItem08.Rate
                    });
                }
                else if (l.RewardItem08.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem08.Num,
                        Probability = l.RewardItem08.Rate
                    });
                }
                else if (l.RewardItem08.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem08.Num,
                        Probability = l.RewardItem08.Rate,
                    });
                }

                //RewardItem09
                if (l.RewardItem09.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem09.ItemID,
                        Amount = l.RewardItem09.Num,
                        Probability = l.RewardItem09.Rate
                    });
                }
                else if (l.RewardItem09.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem09.Num,
                        Probability = l.RewardItem09.Rate
                    });
                }
                else if (l.RewardItem09.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem09.Num,
                        Probability = l.RewardItem09.Rate,
                    });
                }

                //RewardItem10
                if (l.RewardItem10.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem10.ItemID,
                        Amount = l.RewardItem10.Num,
                        Probability = l.RewardItem10.Rate
                    });
                }
                else if (l.RewardItem10.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem10.Num,
                        Probability = l.RewardItem10.Rate
                    });
                }
                else if (l.RewardItem10.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem10.Num,
                        Probability = l.RewardItem10.Rate,
                    });
                }

                //RewardItem11
                if (l.RewardItem11.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem11.ItemID,
                        Amount = l.RewardItem11.Num,
                        Probability = l.RewardItem11.Rate
                    });
                }
                else if (l.RewardItem11.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem11.Num,
                        Probability = l.RewardItem11.Rate
                    });
                }
                else if (l.RewardItem11.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem11.Num,
                        Probability = l.RewardItem11.Rate,
                    });
                }

                //RewardItem12
                if (l.RewardItem12.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem12.ItemID,
                        Amount = l.RewardItem12.Num,
                        Probability = l.RewardItem12.Rate
                    });
                }
                else if (l.RewardItem12.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem12.Num,
                        Probability = l.RewardItem12.Rate
                    });
                }
                else if (l.RewardItem12.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem12.Num,
                        Probability = l.RewardItem12.Rate,
                    });
                }

                //RewardItem13
                if (l.RewardItem13.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem13.ItemID,
                        Amount = l.RewardItem13.Num,
                        Probability = l.RewardItem13.Rate
                    });
                }
                else if (l.RewardItem13.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem13.Num,
                        Probability = l.RewardItem13.Rate
                    });
                }
                else if (l.RewardItem13.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem13.Num,
                        Probability = l.RewardItem13.Rate,
                    });
                }

                //RewardItem14
                if (l.RewardItem14.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem14.ItemID,
                        Amount = l.RewardItem14.Num,
                        Probability = l.RewardItem14.Rate
                    });
                }
                else if (l.RewardItem14.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem14.Num,
                        Probability = l.RewardItem14.Rate
                    });
                }
                else if (l.RewardItem14.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem14.Num,
                        Probability = l.RewardItem14.Rate,
                    });
                }

                //RewardItem15
                if (l.RewardItem15.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem15.ItemID,
                        Amount = l.RewardItem15.Num,
                        Probability = l.RewardItem15.Rate
                    });
                }
                else if (l.RewardItem15.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem15.Num,
                        Probability = l.RewardItem15.Rate
                    });
                }
                else if (l.RewardItem15.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem15.Num,
                        Probability = l.RewardItem15.Rate,
                    });
                }

                //RewardItem16
                if (l.RewardItem16.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem16.ItemID,
                        Amount = l.RewardItem16.Num,
                        Probability = l.RewardItem16.Rate
                    });
                }
                else if (l.RewardItem16.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem16.Num,
                        Probability = l.RewardItem16.Rate
                    });
                }
                else if (l.RewardItem16.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem16.Num,
                        Probability = l.RewardItem16.Rate,
                    });
                }

                //RewardItem17
                if (l.RewardItem17.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem17.ItemID,
                        Amount = l.RewardItem17.Num,
                        Probability = l.RewardItem17.Rate
                    });
                }
                else if (l.RewardItem17.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem17.Num,
                        Probability = l.RewardItem17.Rate
                    });
                }
                else if (l.RewardItem17.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem17.Num,
                        Probability = l.RewardItem17.Rate,
                    });
                }

                //RewardItem18
                if (l.RewardItem18.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem18.ItemID,
                        Amount = l.RewardItem18.Num,
                        Probability = l.RewardItem18.Rate
                    });
                }
                else if (l.RewardItem18.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem18.Num,
                        Probability = l.RewardItem18.Rate
                    });
                }
                else if (l.RewardItem18.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem18.Num,
                        Probability = l.RewardItem18.Rate,
                    });
                }

                //RewardItem19
                if (l.RewardItem19.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem19.ItemID,
                        Amount = l.RewardItem19.Num,
                        Probability = l.RewardItem19.Rate
                    });
                }
                else if (l.RewardItem19.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem19.Num,
                        Probability = l.RewardItem19.Rate
                    });
                }
                else if (l.RewardItem19.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem19.Num,
                        Probability = l.RewardItem19.Rate,
                    });
                }

                //RewardItem20
                if (l.RewardItem20.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem20.ItemID,
                        Amount = l.RewardItem20.Num,
                        Probability = l.RewardItem20.Rate
                    });
                }
                else if (l.RewardItem20.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem20.Num,
                        Probability = l.RewardItem20.Rate
                    });
                }
                else if (l.RewardItem20.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem20.Num,
                        Probability = l.RewardItem20.Rate,
                    });
                }

                //RewardItem21
                if (l.RewardItem21.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem21.ItemID,
                        Amount = l.RewardItem21.Num,
                        Probability = l.RewardItem21.Rate
                    });
                }
                else if (l.RewardItem21.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem21.Num,
                        Probability = l.RewardItem21.Rate
                    });
                }
                else if (l.RewardItem21.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem21.Num,
                        Probability = l.RewardItem21.Rate,
                    });
                }

                //RewardItem22
                if (l.RewardItem22.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem22.ItemID,
                        Amount = l.RewardItem22.Num,
                        Probability = l.RewardItem22.Rate
                    });
                }
                else if (l.RewardItem22.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem22.Num,
                        Probability = l.RewardItem22.Rate
                    });
                }
                else if (l.RewardItem22.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem22.Num,
                        Probability = l.RewardItem22.Rate,
                    });
                }

                //RewardItem23
                if (l.RewardItem23.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem23.ItemID,
                        Amount = l.RewardItem23.Num,
                        Probability = l.RewardItem23.Rate
                    });
                }
                else if (l.RewardItem23.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem23.Num,
                        Probability = l.RewardItem23.Rate
                    });
                }
                else if (l.RewardItem23.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem23.Num,
                        Probability = l.RewardItem23.Rate,
                    });
                }

                //RewardItem24
                if (l.RewardItem24.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem24.ItemID,
                        Amount = l.RewardItem24.Num,
                        Probability = l.RewardItem24.Rate
                    });
                }
                else if (l.RewardItem24.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem24.Num,
                        Probability = l.RewardItem24.Rate
                    });
                }
                else if (l.RewardItem24.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem24.Num,
                        Probability = l.RewardItem24.Rate,
                    });
                }

                //RewardItem25
                if (l.RewardItem25.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem25.ItemID,
                        Amount = l.RewardItem25.Num,
                        Probability = l.RewardItem25.Rate
                    });
                }
                else if (l.RewardItem25.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem25.Num,
                        Probability = l.RewardItem25.Rate
                    });
                }
                else if (l.RewardItem25.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem25.Num,
                        Probability = l.RewardItem25.Rate,
                    });
                }

                //RewardItem26
                if (l.RewardItem26.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem26.ItemID,
                        Amount = l.RewardItem26.Num,
                        Probability = l.RewardItem26.Rate
                    });
                }
                else if (l.RewardItem26.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem26.Num,
                        Probability = l.RewardItem26.Rate
                    });
                }
                else if (l.RewardItem26.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem26.Num,
                        Probability = l.RewardItem26.Rate,
                    });
                }

                //RewardItem27
                if (l.RewardItem27.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem27.ItemID,
                        Amount = l.RewardItem27.Num,
                        Probability = l.RewardItem27.Rate
                    });
                }
                else if (l.RewardItem27.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem27.Num,
                        Probability = l.RewardItem27.Rate
                    });
                }
                else if (l.RewardItem27.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem27.Num,
                        Probability = l.RewardItem27.Rate,
                    });
                }

                //RewardItem28
                if (l.RewardItem28.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem28.ItemID,
                        Amount = l.RewardItem28.Num,
                        Probability = l.RewardItem28.Rate
                    });
                }
                else if (l.RewardItem28.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem28.Num,
                        Probability = l.RewardItem28.Rate
                    });
                }
                else if (l.RewardItem28.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem28.Num,
                        Probability = l.RewardItem28.Rate,
                    });
                }

                //RewardItem29
                if (l.RewardItem29.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem29.ItemID,
                        Amount = l.RewardItem29.Num,
                        Probability = l.RewardItem29.Rate,
                    });
                }
                else if (l.RewardItem29.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem29.Num,
                        Probability = l.RewardItem29.Rate
                    });
                }
                else if (l.RewardItem29.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem29.Num,
                        Probability = l.RewardItem29.Rate,
                    });
                }
                if (items.Count > 0)
                {
                    items.ElementAt(0).Aux = totalrate;
                    table.Add(hash, items);
                }
            }
            return table;
        }
    }
}