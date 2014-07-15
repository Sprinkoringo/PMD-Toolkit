using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Maps {
    public class Tile {


        public TileData Data;

        public Tile()
        {
            Data = new TileData(Enums.TileType.Void, 0, 0, 0, "", "", "");
        }

        public Tile(Tile tile) {
            this.Data = tile.Data;
        }

        public Tile(Enums.TileType type, int data1, int data2, int data3) {
            Data = new TileData(type, data1, data2, data3, "", "", "");
        }

        public Tile(Enums.TileType type, int data1, int data2, int data3, string tileString1, string tileString2, string tileString3)
        {
            Data = new TileData(type, data1, data2, data3, tileString1, tileString2, tileString3);
        }

    }
}
