using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;

namespace TeraFinder
{
    public static class ImagesUtil
    {
        public static Image GetRaidResultSprite(TeraDetails pkm, bool active = true)
        {
            SpriteName.AllowShinySprite = true;
            var file = pkm.Shiny > TeraShiny.No && pkm.Species <= (ushort)Species.Sprigatito ?
                'b' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (int)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, true) :
                'a' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (int)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, false);

            var sprite = (Image?)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file);

            if (sprite is null)
            {
                file = 'a' + SpriteName.GetResourceStringSprite(pkm.Species, pkm.Form, (int)pkm.Gender, (uint)(pkm.Species == (ushort)Species.Gholdengo ? 999 : 0), EntityContext.Gen9, false);
                sprite = (Image)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(file)!;
            }

            if (pkm.Shiny > TeraShiny.No)
                sprite = LayerOverImageShiny(sprite, pkm.Shiny is TeraShiny.Square ? Shiny.AlwaysSquare : Shiny.AlwaysStar);

            if (!active) sprite = ImageUtil.ToGrayscale(sprite);
            return ImageUtil.BlendTransparentTo(sprite, TypeColor.GetTypeSpriteColor((byte)pkm.TeraType), 0xAF, 0x3740);
        }

        private static Image LayerOverImageShiny(Image image, Shiny shiny)
        {
            var icon = shiny is Shiny.AlwaysSquare ? PKHeX.Drawing.PokeSprite.Properties.Resources.rare_icon_2 : PKHeX.Drawing.PokeSprite.Properties.Resources.rare_icon;
            return ImageUtil.LayerImage(image, icon, 0, 0, 0.7);
        }

        public static void SetMapPoint(this PictureBox pic, int teratype, int area, int spawnpoint, Dictionary<string, float[]> locations)
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

            var loc_available = locations.TryGetValue($"{area}-{spawnpoint}", out var location);
            var coordinates = new Point
            {
                X = loc_available ? (int)((location![0] - 52) * pic.Width / 5000) : 0,
                Y = loc_available ? (int)((location![2] + 5178) * pic.Height / 5000) : 0,
            };

            var pointer = new Bitmap(crystal, new Size(crystal.Width / 4, crystal.Height / 4));
            var map = new Bitmap(pic.BackgroundImage!, new Size(pic.Width, pic.Height));
            if(coordinates.X != 0 && coordinates.Y != 0)
                Graphics.FromImage(map).DrawImage(pointer, coordinates);
            pic.Image = map;
        }

        public static void ResetMap(this PictureBox pic)
        {
            var map = new Bitmap(pic.BackgroundImage!, new Size(pic.Width, pic.Height));
            pic.Image = map;
        }
    }
}