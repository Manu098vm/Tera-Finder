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
            'b' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (int)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, true) :
            'a' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (int)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, false);

        var sprite = (Image?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);

        if (sprite is null)
        {
            file = 'a' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (int)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, false);
            sprite = (Image?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);
        }

        if (item > 0 && sprite is not null)
            sprite = LayerOverImageItem(sprite, item);

        if (pkm.Shiny > TeraShiny.No && sprite is not null)
            sprite = LayerOverImageShiny(sprite, pkm.Shiny is TeraShiny.Square ? Shiny.AlwaysSquare : Shiny.AlwaysStar);

        if (!active && sprite is not null) sprite = ImageUtil.ToGrayscale(sprite);

        if(sprite is not null)
            ImageUtil.BlendTransparentTo(sprite, TypeColor.GetTypeSpriteColor((byte)pkm.TeraType), 0xAF, 0x3740);

        return sprite;
    }

    public static Image? GetSimpleSprite(ushort species, byte form, bool active)
    {
        SpriteName.AllowShinySprite = false;
        var file = 'a' + SpriteName.GetResourceStringSprite(species, form, 0, 0, EntityContext.Gen9, false);
        var sprite = (Image?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);
        if (!active && sprite is not null) sprite = ImageUtil.ToGrayscale(sprite);
        return sprite;
    }

    private static Image LayerOverImageItem(Image image, int item)
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

    private static Image LayerOverImageShiny(Image image, Shiny shiny)
    {
        var icon = shiny is Shiny.AlwaysSquare ? PKHeX.Drawing.PokeSprite.Properties.Resources.rare_icon_2 : PKHeX.Drawing.PokeSprite.Properties.Resources.rare_icon;
        return ImageUtil.LayerImage(image, icon, 0, 0, 0.7);
    }

    public static void SetMapPoint(this PictureBox pic, string multipleden, int teratype, int area, int spawnpoint, TeraRaidMapParent map, Dictionary<string, float[]> locations)
    {
        var loc_available = locations.TryGetValue($"{area}-{spawnpoint}", out var location);
        var x = loc_available ? location![0] : 0;
        var y = loc_available ? location![2] : 0;

        var loc2_available = locations.TryGetValue($"{area}-{spawnpoint}_", out var location2);
        var x2 = loc2_available ? location2![0] : 0;
        var y2 = loc2_available ? location2![2] : 0;

        var loc3_available = locations.TryGetValue($"{area}-{spawnpoint}__", out var location3);
        var x3 = loc3_available ? location3![0] : 0;
        var y3 = loc3_available ? location3![2] : 0;

        if (loc3_available)
            pic.SetMapPoint(map, teratype, multipleden, x, y, x2, y2, x3, y3);
        if (loc2_available)
            pic.SetMapPoint(map, teratype, multipleden, x, y, x2, y2);
        else if (loc_available)
            pic.SetMapPoint(map, teratype, multipleden, x, y);
        else 
            pic.SetMapPoint(map, teratype, $"Missing Coordinates{Environment.NewLine}Area ID: {area}{Environment.NewLine}Den ID: {spawnpoint}", missing: true);
    }

    public static void SetMapPoint(this PictureBox pic, TeraRaidMapParent map, string multipleden, GameCoordinates coordinates, int teratype = 0) =>
        pic.SetMapPoint(map, teratype, multipleden, coordinates.X, coordinates.Z);

    private static void SetMapPoint(this PictureBox pic, TeraRaidMapParent map, int teratype, string message, 
        float x = 0, float y = 0, float x2 = 0, float y2 = 0, float x3 = 0, float y3 = 0, bool missing = false)
    {
        var crystal = (MoveType)teratype switch
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

        // Formulas taken from RaidCrawler (GPL v3 License)
        // Thx Lincoln, Lego and Archit!
        // https://github.com/LegoFigure11/RaidCrawler/blob/d36475046c638fbc37fbeb0aaa001f3663273b9b/RaidCrawler.WinForms/MainWindow.cs#L1589

        var coordinates = new Point
        {
            X = x != 0 ? (int)((((map switch { TeraRaidMapParent.Kitakami => 2.766970605475146, _ => 1 }) * x) + (map switch { TeraRaidMapParent.Kitakami => 2.072021484, _ => -248.08352352566726 })) * 512 / 5000) : 0,
            Y = y != 0 ? (int)((((map switch { TeraRaidMapParent.Kitakami => 2.5700782642623805, _ => 1 }) * y) + (map switch { TeraRaidMapParent.Kitakami => 5070.808599816581, _ => 5505.240018 })) * 512 / 5000) : 0,
        };
       
        var coordinates2 = new Point
        {
            X = x2 != 0 ? (int)((((map switch { TeraRaidMapParent.Kitakami => 2.766970605475146, _ => 1 }) * x2) + (map switch { TeraRaidMapParent.Kitakami => 2.072021484, _ => -248.08352352566726 })) * 512 / 5000) : 0,
            Y = y2 != 0 ? (int)((((map switch { TeraRaidMapParent.Kitakami => 2.5700782642623805, _ => 1 }) * y2) + (map switch { TeraRaidMapParent.Kitakami => 5070.808599816581, _ => 5505.240018 })) * 512 / 5000) : 0,
        };

        var coordinates3 = new Point
        {
            X = x3 != 0 ? (int)((((map switch { TeraRaidMapParent.Kitakami => 2.766970605475146, _ => 1 }) * x3) + (map switch { TeraRaidMapParent.Kitakami => 2.072021484, _ => -248.08352352566726 })) * 512 / 5000) : 0,
            Y = y3 != 0 ? (int)((((map switch { TeraRaidMapParent.Kitakami => 2.5700782642623805, _ => 1 }) * y3) + (map switch { TeraRaidMapParent.Kitakami => 5070.808599816581, _ => 5505.240018 })) * 512 / 5000) : 0,
        };

        var pointer = new Bitmap(crystal, new Size(crystal.Width / 4, crystal.Height / 4));
        var mapImage = new Bitmap(pic.BackgroundImage!, new Size(pic.Width, pic.Height));

        if (coordinates.X != 0 && coordinates.Y != 0)
            Graphics.FromImage(mapImage).DrawImage(pointer, coordinates);

        if (coordinates2.X != 0 && coordinates2.Y != 0)
            Graphics.FromImage(mapImage).DrawImage(pointer, coordinates2);

        if (coordinates3.X != 0 && coordinates3.Y != 0)
            Graphics.FromImage(mapImage).DrawImage(pointer, coordinates2);

        if (coordinates.X != 0 && coordinates.Y != 0 && coordinates2.X != 0 && coordinates2.Y != 0 && 
            coordinates.X != coordinates2.X && coordinates.Y != coordinates2.Y)
            Graphics.FromImage(mapImage).DrawString(message, new Font("Arial", 14), new SolidBrush(Color.Black), new PointF(0, 0));

        if (missing)
            Graphics.FromImage(mapImage).DrawString(message, new Font("Arial", 14), new SolidBrush(Color.Black), new PointF(0, 0));

        pic.Image = mapImage;
    }

    public static void ResetMap(this PictureBox pic)
    {
        var map = new Bitmap(pic.BackgroundImage!, new Size(pic.Width, pic.Height));
        pic.Image = map;
    }
}