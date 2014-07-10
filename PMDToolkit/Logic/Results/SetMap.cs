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
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SetMap : IResult {

        //public ResultType Type { get { return ResultType.SetMap; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        DisplayMap map;
        int floor;

        public SetMap(ActiveMap map, int floor) {
            this.map = new DisplayMap();
            this.map.MapArray = new Tile[map.Width, map.Height];
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    this.map.MapArray[x, y] = new Tile(map.MapArray[x, y]);
                }
            }

            this.map.GroundLayers = new List<TileAnim[,]>();
            for (int i = 0; i < map.GroundLayers.Count; i++)
            {
                this.map.GroundLayers.Add(new TileAnim[map.Width, map.Height]);
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        this.map.GroundLayers[i][x, y] = map.GroundLayers[i].Tiles[x, y];
                    }
                }
            }

            this.map.PropBackLayers = new List<TileAnim[,]>();
            for (int i = 0; i < map.PropBackLayers.Count; i++)
            {
                this.map.PropBackLayers.Add(new TileAnim[map.Width, map.Height]);
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        this.map.PropBackLayers[i][x, y] = map.PropBackLayers[i].Tiles[x, y];
                    }
                }
            }

            this.map.PropFrontLayers = new List<TileAnim[,]>();
            for (int i = 0; i < map.PropFrontLayers.Count; i++)
            {
                this.map.PropFrontLayers.Add(new TileAnim[map.Width, map.Height]);
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        this.map.PropFrontLayers[i][x, y] = map.PropFrontLayers[i].Tiles[x, y];
                    }
                }
            }

            this.map.FringeLayers = new List<TileAnim[,]>();
            for (int i = 0; i < map.FringeLayers.Count; i++)
            {
                this.map.FringeLayers.Add(new TileAnim[map.Width, map.Height]);
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        this.map.FringeLayers[i][x, y] = map.FringeLayers[i].Tiles[x, y];
                    }
                }
            }
                        
            this.floor = floor;
        }

        public void Execute() {
            Screen.Map = map;
            Screen.Floor = floor;
        }

    }
}
