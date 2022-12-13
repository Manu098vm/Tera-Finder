using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;

namespace TeraFinder
{
    public static class SpriteUtil
    {
        public static Image GetRaidResultSprite(TeraDetails pkm, bool active = true)
        {
            var sprite = PKHeX.Drawing.PokeSprite.SpriteUtil.GetSprite((ushort)pkm.Species, (byte)pkm.Form, (int)pkm.Gender, pkm.Species is Species.Gholdengo ? (uint)999 : 0, 0, false, (Shiny)pkm.Shiny, 9, SpriteBuilderTweak.None);
            if (!active) sprite = ImageUtil.ToGrayscale(sprite);
            return ImageUtil.BlendTransparentTo(sprite, TypeColor.GetTypeSpriteColor((byte)pkm.TeraType), 0xAF, 0x3740);
        }
    }
}
