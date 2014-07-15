using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SetTile : IResult {

        //public ResultType Type { get { return ResultType.SetTile; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        Loc2D loc;
        Tile tile;
        public List<TileAnim> groundLayers;
        public List<TileAnim> propBackLayers;
        public List<TileAnim> propFrontLayers;
        public List<TileAnim> fringeLayers;

        public SetTile(BasicMap map, Loc2D loc) {
            this.loc = loc;
            this.tile = new Tile(map.MapArray[loc.X, loc.Y]);
            groundLayers = new List<TileAnim>();
            for (int i = 0; i < map.GroundLayers.Count; i++)
            {
                groundLayers.Add(map.GroundLayers[i].Tiles[loc.X, loc.Y]);
            }
            propBackLayers = new List<TileAnim>();
            for (int i = 0; i < map.PropBackLayers.Count; i++)
            {
                propBackLayers.Add(map.PropBackLayers[i].Tiles[loc.X, loc.Y]);
            }
            propFrontLayers = new List<TileAnim>();
            for (int i = 0; i < map.PropFrontLayers.Count; i++)
            {
                propFrontLayers.Add(map.PropFrontLayers[i].Tiles[loc.X, loc.Y]);
            }
            fringeLayers = new List<TileAnim>();
            for (int i = 0; i < map.FringeLayers.Count; i++)
            {
                fringeLayers.Add(map.FringeLayers[i].Tiles[loc.X, loc.Y]);
            }
        }

        public void Execute() {
            Screen.Map.MapArray[loc.X, loc.Y] = tile;
            for (int i = 0; i < groundLayers.Count; i++)
            {
                Screen.Map.GroundLayers[i][loc.X, loc.Y] = groundLayers[i];
            }
            for (int i = 0; i < propBackLayers.Count; i++)
            {
                Screen.Map.PropBackLayers[i][loc.X, loc.Y] = propBackLayers[i];
            }
            for (int i = 0; i < propFrontLayers.Count; i++)
            {
                Screen.Map.PropFrontLayers[i][loc.X, loc.Y] = propFrontLayers[i];
            }
            for (int i = 0; i < fringeLayers.Count; i++)
            {
                Screen.Map.FringeLayers[i][loc.X, loc.Y] = fringeLayers[i];
            }
        }
    }
}
