using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Logic.Display {
    public class DisplayMap {

        public Tile[,] MapArray { get; set; }

        public List<TileAnim[,]> GroundLayers { get; set; }
        public List<TileAnim[,]> PropBackLayers { get; set; }
        public List<TileAnim[,]> PropFrontLayers { get; set; }
        public List<TileAnim[,]> FringeLayers { get; set; }
        
        public int Width { get { return MapArray.GetLength(0); } }
        public int Height { get { return MapArray.GetLength(1); } }

        public DisplayMap()
        {
            MapArray = new Tile[1, 1];
            MapArray[0, 0] = new Tile(Enums.TileType.Void, 0, 0, 0);
            GroundLayers = new List<TileAnim[,]>();
            PropBackLayers = new List<TileAnim[,]>();
            PropFrontLayers = new List<TileAnim[,]>();
            FringeLayers = new List<TileAnim[,]>();
        }

        public void Draw() {


        }


        public void DrawGround(int x, int y, Loc2D loc)
        {
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(x, y, 0));
            TextureManager.TextureProgram.UpdateModelView();
            for (int i = 0; i < GroundLayers.Count; i++)
            {
                bool drawLayer = true;

                if (Editors.MapEditor.mapEditing && !Editors.MapEditor.showGroundLayer[i])
                    drawLayer = false;

                if (drawLayer)
                    GroundLayers[i][loc.X, loc.Y].Draw(Screen.TotalTick);
            }
            TextureManager.TextureProgram.PopModelView();
        }


        public void DrawPropBack(int x, int y, Loc2D loc)
        {
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(x, y, 0));
            TextureManager.TextureProgram.UpdateModelView();

            for (int i = 0; i < PropBackLayers.Count; i++)
            {
                bool drawLayer = true;

                if (Editors.MapEditor.mapEditing && !Editors.MapEditor.showPropBackLayer[i])
                    drawLayer = false;

                if (drawLayer)
                    PropBackLayers[i][loc.X, loc.Y].Draw(Screen.TotalTick);
            }
            TextureManager.TextureProgram.PopModelView();
        }

        public void DrawPropFront(int x, int y, Loc2D loc)
        {
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(x, y, 0));
            TextureManager.TextureProgram.UpdateModelView();

            for (int i = 0; i < PropFrontLayers.Count; i++)
            {
                bool drawLayer = true;

                if (Editors.MapEditor.mapEditing && !Editors.MapEditor.showPropFrontLayer[i])
                    drawLayer = false;

                if (drawLayer)
                    PropFrontLayers[i][loc.X, loc.Y].Draw(Screen.TotalTick);
            }
            TextureManager.TextureProgram.PopModelView();
        }

        public void DrawFringe(int x, int y, Loc2D loc)
        {
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(x, y, 0));
            TextureManager.TextureProgram.UpdateModelView();

            for (int i = 0; i < FringeLayers.Count; i++)
            {
                bool drawLayer = true;

                if (Editors.MapEditor.mapEditing && !Editors.MapEditor.showFringeLayer[i])
                    drawLayer = false;

                if (drawLayer)
                    FringeLayers[i][loc.X, loc.Y].Draw(Screen.TotalTick);
            }
            TextureManager.TextureProgram.PopModelView();
        }
    }
}
