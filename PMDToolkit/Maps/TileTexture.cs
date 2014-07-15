using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct TileTexture {

        public Loc2D Texture;
        public int Sheet;

        
        public TileTexture(Loc2D texture, int sheet) {
            Texture = texture;
            Sheet = sheet;
        }

        public static bool operator ==(TileTexture param1, TileTexture param2)
        {
            return (param1.Texture == param2.Texture && param1.Sheet == param2.Sheet);
        }

        public static bool operator !=(TileTexture param1, TileTexture param2)
        {
            return !(param1 == param2);
        }

        public void Draw() {
            Graphics.Texture texture = Graphics.TextureManager.GetTile(Sheet, Texture);
            texture.Render(null);
        }

    }
}
