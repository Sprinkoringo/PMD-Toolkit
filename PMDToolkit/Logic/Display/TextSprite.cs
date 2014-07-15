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
    public class TextSprite : ISprite {

        static readonly RenderTime TOTAL_ANIM_TIME = RenderTime.FromMillisecs(128000);
        const int NUM_SCALE = 2;

        public Loc2D EffectLoc { get; set; }
        public Loc2D MapLoc { get { return new Loc2D(EffectLoc.X * TextureManager.TILE_SIZE, EffectLoc.Y * TextureManager.TILE_SIZE); } }
        public int MapHeight { get; set; }
        public RenderTime ActionTime { get; set; }
        public string Text { get; set; }
        public bool ActionDone { get; set; }
        public Color4 Color { get; set; }

        public TextSprite(Loc2D loc, string text, Color4 color) {
            EffectLoc = loc;
            Text = text;
            ActionDone = false;
            Color = color;
        }


        public virtual void Begin()
        {

        }

        public virtual void Process(RenderTime elapsedTime)
        {
            ActionTime += elapsedTime;
            if (ActionTime >= TOTAL_ANIM_TIME) {
                ActionDone = true;
            } else {
                MapHeight = ActionTime.Ticks * TextureManager.TILE_SIZE * 6 / TOTAL_ANIM_TIME.Ticks;
                if (MapHeight > TextureManager.TILE_SIZE) MapHeight = TextureManager.TILE_SIZE;
            }
        }

        public virtual void Draw() {
            TextureManager.TextureProgram.PushModelView();
            Loc2D drawLoc = GetStart();
            Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(drawLoc.X, drawLoc.Y, 0));
            Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale(NUM_SCALE, NUM_SCALE, 0));
            Graphics.TextureManager.TextureProgram.UpdateModelView();

            TextureManager.SingleFont.RenderText(0, 0, Text, null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Left, 0, Color);

            TextureManager.TextureProgram.PopModelView();
        }

        public Loc2D GetStart() {
            return new Loc2D(EffectLoc.X * Graphics.TextureManager.TILE_SIZE + Graphics.TextureManager.TILE_SIZE / 2 - (int)TextureManager.SingleFont.SubstringWidth(Text) * NUM_SCALE / 2,
                EffectLoc.Y * Graphics.TextureManager.TILE_SIZE - MapHeight + Graphics.TextureManager.TILE_SIZE / 2 - (int)TextureManager.SingleFont.StringHeight(Text, 0) * NUM_SCALE / 2);
        }

        public Loc2D GetEnd() {
            return new Loc2D(EffectLoc.X * Graphics.TextureManager.TILE_SIZE + Graphics.TextureManager.TILE_SIZE / 2 + (int)TextureManager.SingleFont.SubstringWidth(Text) * NUM_SCALE / 2,
                EffectLoc.Y * Graphics.TextureManager.TILE_SIZE - MapHeight + Graphics.TextureManager.TILE_SIZE / 2 + (int)TextureManager.SingleFont.StringHeight(Text, 0) * NUM_SCALE / 2);
        }

    }
}
