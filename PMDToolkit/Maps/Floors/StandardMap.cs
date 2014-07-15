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
