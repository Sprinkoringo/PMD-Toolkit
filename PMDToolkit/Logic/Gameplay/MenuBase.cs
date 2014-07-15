using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Gameplay {
    public abstract class MenuBase {

        public Loc2D Start { get; set; }
        public Loc2D End { get; set; }

        public bool Visible { get; set; }

        public List<MenuText> NonChoices { get; set; }
        public List<MenuText> Choices { get; set; }

        public abstract Loc2D PickerPos { get; }

        public MenuBase() {
            NonChoices = new List<MenuText>();
            Choices = new List<MenuText>();
            Visible = true;
        }

        public abstract void Process(Input input, ActiveChar character, ref bool moveMade);

        public void Draw() {
            if (!Visible) return;

            //draw background
            //top-left
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(Start.X, Start.Y, 0));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(0, 0);
            TextureManager.TextureProgram.PopModelView();
            //top-right
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(End.X - Graphics.TextureManager.MenuBack.TileWidth, Start.Y, 0));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(2, 0);
            TextureManager.TextureProgram.PopModelView();
            //bottom-right
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(End.X - Graphics.TextureManager.MenuBack.TileWidth, End.Y - Graphics.TextureManager.MenuBack.TileHeight, 0));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(2, 2);
            TextureManager.TextureProgram.PopModelView();
            //bottom-left
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(Start.X, End.Y - Graphics.TextureManager.MenuBack.TileHeight, 0));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(0, 2);
            TextureManager.TextureProgram.PopModelView();

            //top
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(Start.X + Graphics.TextureManager.MenuBack.TileWidth, Start.Y, 0));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale((float)(End.X - Start.X - 2 * Graphics.TextureManager.MenuBack.TileWidth) / Graphics.TextureManager.MenuBack.TileWidth, 1, 1));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(1, 0);
            TextureManager.TextureProgram.PopModelView();

            //right
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(End.X - Graphics.TextureManager.MenuBack.TileWidth, Start.Y + Graphics.TextureManager.MenuBack.TileHeight, 0));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale(1, (float)(End.Y - Start.Y - 2 * Graphics.TextureManager.MenuBack.TileHeight) / Graphics.TextureManager.MenuBack.TileHeight, 1));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(2, 1);
            TextureManager.TextureProgram.PopModelView();

            //bottom
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(Start.X + Graphics.TextureManager.MenuBack.TileWidth, End.Y - Graphics.TextureManager.MenuBack.TileHeight, 0));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale((float)(End.X - Start.X - 2 * Graphics.TextureManager.MenuBack.TileWidth) / Graphics.TextureManager.MenuBack.TileWidth, 1, 1));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(1, 2);
            TextureManager.TextureProgram.PopModelView();

            //left
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(Start.X, Start.Y + Graphics.TextureManager.MenuBack.TileHeight, 0));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale(1, (float)(End.Y - Start.Y - 2 * Graphics.TextureManager.MenuBack.TileHeight) / Graphics.TextureManager.MenuBack.TileHeight, 1));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(0, 1);
            TextureManager.TextureProgram.PopModelView();

            //center
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(Start.X + Graphics.TextureManager.MenuBack.TileWidth, Start.Y + Graphics.TextureManager.MenuBack.TileHeight, 0));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale((float)(End.X - Start.X - 2 * Graphics.TextureManager.MenuBack.TileWidth) / Graphics.TextureManager.MenuBack.TileWidth,
                (float)(End.Y - Start.Y - 2 * Graphics.TextureManager.MenuBack.TileHeight) / Graphics.TextureManager.MenuBack.TileHeight, 1));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.MenuBack.RenderTile(1, 1);
            TextureManager.TextureProgram.PopModelView();

            //draw choices
            for (int i = 0; i < Choices.Count; i++) {
                TextureManager.SingleFont.RenderText(Choices[i].Loc.X, Choices[i].Loc.Y, Choices[i].Text, null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Left, 0, Choices[i].Color);
            }

            //draw picker
            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(PickerPos.X, PickerPos.Y, 0));
            TextureManager.TextureProgram.UpdateModelView();
            Graphics.TextureManager.Picker.Render(null);
            TextureManager.TextureProgram.PopModelView();

        }
    }
}
