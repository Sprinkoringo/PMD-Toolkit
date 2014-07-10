/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit;
using PMDToolkit.Maps;
using PMDToolkit.Data;
using PMDToolkit.Logic.Gameplay;

namespace PMDToolkit.Maps.Floors {
    public class StandardMap : RandomMap {

        public StandardMap()
        {
            
        }

        //an initial create-map method
        public override void Generate(int seed, RDungeonFloor entry, List<FloorBorder> floorBorders, Dictionary<int, List<int>> borderLinks) {
            //TODO: make sure that this algorithm follows floorBorders and borderLinks constraints

            this.seed = seed;
            this.entry = entry;
            FloorBorders = floorBorders;
            BorderLinks = borderLinks;

            BorderPoints = new Loc2D[1];

            rand = new Random(seed);

            MapArray = new Tile[10, 10];


            MapLayer ground = new MapLayer(Width, Height);
            GroundLayers.Add(ground);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.Walkable, 0, 0, 0);
                    GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                }
            }

            BorderPoints[0] = new Loc2D(0, 0);
        }


        public void Generate(int seed, string fileName)
        {
            //TODO: make sure that this algorithm follows floorBorders and borderLinks constraints

            this.seed = seed;
            this.entry = entry;
            FloorBorders = null;
            BorderLinks = null;

            BorderPoints = new Loc2D[1];

            rand = new Random(seed);

            //load from file

            MapArray = new Tile[100, 100];
            
            MapLayer ground = new MapLayer(Width, Height);
            GroundLayers.Add(ground);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.Walkable, 0, 0, 0);
                    GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                }
            }

            BorderPoints[0] = new Loc2D(99, 0);
        }
        
    }
}
