using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps
{
    public class MapLayer
    {
        public TileAnim[,] Tiles;

        public string Name { get; set; }

        public MapLayer(int width, int height)
        {
            Name = "";
            Tiles = new TileAnim[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tiles[x, y] = new TileAnim();
                }
            }
        }

        public MapLayer(string name, int width, int height)
        {
            Name = name;
            Tiles = new TileAnim[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tiles[x, y] = new TileAnim();
                }
            }
        }

        public void Resize(int width, int height, Maps.Direction8 dir)
        {
            Operations.ResizeArray<TileAnim>(ref Tiles, width, height, dir, true);
        }
    }
}
