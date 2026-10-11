using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public static class ImagesUtil
{
    public static Image? GetRaidResultSprite(TeraDetails pkm, bool active = true, int item = 0)
    {
        SpriteName.AllowShinySprite = true;
        var file = pkm.Shiny > TeraShiny.No && pkm.Species < (ushort)Species.Sprigatito ?
            'b' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (byte)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, true) :
            'a' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (byte)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, false);

        var sprite = (Bitmap?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);

        if (sprite is null)
        {
            file = 'a' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (byte)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, false);
            sprite = (Bitmap?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);
        }

        if (item > 0 && sprite is not null)
            sprite = LayerOverImageItem(sprite, item);

        if (pkm.Shiny > TeraShiny.No && sprite is not null)
            sprite = LayerOverImageShiny(sprite, pkm.Shiny is TeraShiny.Square ? Shiny.AlwaysSquare : Shiny.AlwaysStar);

        if (!active && sprite is not null) ImageUtil.ToGrayscale(sprite,0.7f);

        if(sprite is not null)
            ImageUtil.BlendTransparentTo(sprite, TypeColor.GetTypeSpriteColor(pkm.TeraType), 0xAF, 0x3740);

        return sprite;
    }

    public static Bitmap? GetSimpleSprite(ushort species, byte form, bool active)
    {
        SpriteName.AllowShinySprite = false;
        var file = 'a' + SpriteName.GetResourceStringSprite(species, form, 0, 0, EntityContext.Gen9, false);
        var sprite = (Bitmap?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);
        if (!active && sprite is not null) ImageUtil.ToGrayscale(sprite,0.7f);
        return sprite;
    }

    private static Bitmap LayerOverImageItem(Bitmap image, int item)
    {
        var str = item > 0 ? $"bitem_{item}" : "";
        var icon = (Image?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(str);
        if (icon is not null)
        {
            // Redraw item in bottom right corner; since images are cropped, try to not have them at the edge
            int x = image.Width - icon.Width - ((32 - icon.Width) / 4) - 2;
            int y = image.Height - icon.Height - 2;
            return ImageUtil.LayerImage(image, icon, x, y);
        }
        return image;
    }

    private static Bitmap LayerOverImageShiny(Bitmap image, Shiny shiny)
    {
        var icon = shiny is Shiny.AlwaysSquare ? PKHeX.Drawing.PokeSprite.Properties.Resources.rare_icon_2 : PKHeX.Drawing.PokeSprite.Properties.Resources.rare_icon;
        return ImageUtil.LayerImage(image, icon, 0, 0, 0.7);
    }

    public static void SetMapPoint(this PictureBox pic, MoveType teratype, int area, int lotterygroup, int spawnpoint, TeraRaidMapParent map, Dictionary<string, float[]> locations)
    {
        var loc_available = locations.TryGetValue($"{area}-{lotterygroup}-{spawnpoint}", out var location);
        var x = loc_available ? location![0] : 0;
        var y = loc_available ? location![2] : 0;
        pic.SetMapPoint(map, teratype, x, y);
    }

    public static void SetMapPoint(this PictureBox pic, TeraRaidMapParent map, GameCoordinates coordinates, MoveType teratype = 0) =>
        pic.SetMapPoint(map, teratype, coordinates.X, coordinates.Z);

    private static void SetMapPoint(this PictureBox pic, TeraRaidMapParent map, MoveType teratype, float x, float y)
    {
        var crystal = teratype switch
        {
            MoveType.Normal => Properties.Resources.item_1862,
            MoveType.Fighting => Properties.Resources.item_1868,
            MoveType.Flying => Properties.Resources.item_1871,
            MoveType.Poison => Properties.Resources.item_1869,
            MoveType.Ground => Properties.Resources.item_1870,
            MoveType.Rock => Properties.Resources.item_1874,
            MoveType.Bug => Properties.Resources.item_1873,
            MoveType.Ghost => Properties.Resources.item_1875,
            MoveType.Steel => Properties.Resources.item_1878,
            MoveType.Fire => Properties.Resources.item_1863,
            MoveType.Water => Properties.Resources.item_1864,
            MoveType.Grass => Properties.Resources.item_1866,
            MoveType.Electric => Properties.Resources.item_1865,
            MoveType.Psychic => Properties.Resources.item_1872,
            MoveType.Ice => Properties.Resources.item_1867,
            MoveType.Dragon => Properties.Resources.item_1876,
            MoveType.Dark => Properties.Resources.item_1877,
            MoveType.Fairy => Properties.Resources.item_1879,
            _ => Properties.Resources.item_1862,
        };
        var pointer = new Bitmap(crystal, new Size(crystal.Width / 4, crystal.Height / 4));

        var width = pic.Width;
        var height = pic.Height;

        (double ScaleX, double ScaleZ, double OffsetZ) m = map switch
        {
            TeraRaidMapParent.Blueberry => Blueberry,
            TeraRaidMapParent.Kitakami => Kitakami,
            _ => Paldea,
        };

        var coordinates = new Point()
        {
            X = (int)Normalize(width, ConvertX(x, m.ScaleX) - (pointer.Width / 4), 512),
            Y = (int)Normalize(height, ConvertZ(y, m.ScaleZ, m.OffsetZ) - pointer.Height, 512),
        };

        var mapImage = new Bitmap(pic.BackgroundImage!, new Size(pic.Width, pic.Height));
        Graphics.FromImage(mapImage).DrawImage(pointer, coordinates);
        pic.Image = mapImage;
    }

    private static double Normalize(double x, double y, double z) => x * y / z;

    private static (double, double, double) Paldea = (5000, 5000, 5500);
    private static (double, double, double) Kitakami = (2000, 2000, 2000);
    private static (double, double, double) Blueberry = (2000, 2000, 2000);

    private static double ConvertX(double x, double ScaleX) => (512 / ScaleX) * x;
    private static double ConvertZ(double z, double ScaleZ, double OffsetZ) => (512 / ScaleZ) * (z + OffsetZ);

    public static void ResetMap(this PictureBox pic)
    {
        var map = new Bitmap(pic.BackgroundImage!, new Size(pic.Width, pic.Height));
        pic.Image = map;
    }
}