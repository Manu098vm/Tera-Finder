using PKHeX.Core;

namespace TeraFinder.Core
{
    public interface IOutbreak
    {
        int ID { get; }
        SAV9SV SAV { get; init; }
        sbyte AmountAvailable { get => GetAmountAvailable(); set => SetAmountAvailable(value); }
        string LocationMap { get; set; }

        GameCoordinates? LocationCenter { get; set; }
        GameCoordinates? LocationDummy { get; set; }
        bool Found { get => GetFound(); set => SetFound(value); }
        bool Enabled { get => GetEnabled(); set => SetEnabled(value); }
        uint Species { get => GetSpecies(); set => SetSpecies(value); }
        byte Form { get => GetForm(); set => SetForm(value); }
        int NumKO { get => GetNumKO(); set => SetNumKO(value); }
        int MaxSpawns { get => GetMaxSpawns(); set => SetMaxSpawns(value); }
        public sbyte GetAmountAvailable();
        public void SetAmountAvailable(sbyte value);
        public bool GetEnabled();
        public void SetEnabled(bool value);
        public bool GetFound();
        public void SetFound(bool value);
        public uint GetSpecies();
        public void SetSpecies(uint value);
        public byte GetForm();
        public void SetForm(byte value);
        public int GetNumKO();
        public void SetNumKO(int value);
        public int GetMaxSpawns();
        public void SetMaxSpawns(int value);
        public FakeOutbreak Clone();
        public void DumpTojson(string path);
    }

}