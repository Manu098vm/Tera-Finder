using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.PokeSprite;

namespace TeraRaidEditor
{
    public static class SpriteUtil
    {
        public static Image GetRaidResultSprite(PK9 pkm, bool active = true)
        {
            var shiny = pkm.IsShiny ? (pkm.ShinyXor == 0 ? Shiny.AlwaysSquare : Shiny.AlwaysStar) : Shiny.Never;
            var sprite = PKHeX.Drawing.PokeSprite.SpriteUtil.GetSprite(pkm.Species, pkm.Form, pkm.Gender, pkm.FormArgument, pkm.HeldItem, false, shiny, 9, SpriteBuilderTweak.None);
            if (!active) sprite = ImageUtil.ToGrayscale(sprite);
            return ImageUtil.BlendTransparentTo(sprite, TypeColor.GetTypeSpriteColor((byte)pkm.TeraType), 0xAF, 0x3740);
        }
    }
}
