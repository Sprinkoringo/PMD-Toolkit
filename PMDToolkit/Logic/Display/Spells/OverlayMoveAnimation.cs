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

    class OverlayMoveAnimation : ISpellSprite {
        #region Constructors

        public OverlayMoveAnimation(int animIndex, RenderTime animTime, int loops, byte transparency)
        {
            AnimationIndex = animIndex;
            FrameLength = animTime;
            TotalLoops = loops;
            Alpha = (byte)(255 - transparency);
        }

        #endregion Constructors

        #region Properties

        public int AnimationIndex
        {
            get;
            set;
        }

        public RenderTime FrameTime {
            get;
            set;
        }

        public RenderTime FrameLength {
            get;
            set;
        }

        public int Frame {
            get;
            set;
        }

        //total frames

        public int Loops {
            get;
            set;
        }

        public int TotalLoops
        {
            get;
            set;
        }

        public byte Alpha { get; set; }

        public MoveAnimationType AnimType
        {
            get { return MoveAnimationType.Overlay; }
        }

        public Direction8 Direction { get { return Direction8.None; } }

        public Loc2D StartLoc { get { return new Loc2D(); } }

        public Loc2D MapLoc { get { return new Loc2D(); } }
        public int MapHeight { get; set; }

        public RenderTime ActionTime { get; set; }
        public bool ActionDone { get; set; }


        #endregion Properties

        public virtual void Begin()
        {

        }

        public virtual void Process(RenderTime elapsedTime) {
            ActionTime += elapsedTime;
            FrameTime += elapsedTime;
            if (FrameTime >= FrameLength) {
                FrameTime = FrameTime - FrameLength;
                Frame++;
            }

            if (Frame >= TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TotalFrames) {
                Loops++;
                Frame = 0;
            }

            if (Loops >= TotalLoops) {
                ActionDone = true;
            }
        }

        public virtual void Draw() {
            if (!ActionDone) {
                AnimSheet sheet = TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex);
                TextureManager.TextureProgram.PushModelView();
                TextureManager.TextureProgram.SetTextureColor(new Color4(255, 255, 255, Alpha));
                for (int y = 0; y < Graphics.TextureManager.SCREEN_HEIGHT; y += sheet.TileHeight) {
                    for (int x = 0; x < Graphics.TextureManager.SCREEN_WIDTH; x += sheet.TileWidth) {
                        Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
                        Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(x, y, 0));
                        Graphics.TextureManager.TextureProgram.UpdateModelView();
                        sheet.RenderAnim(Frame, 0, 0);
                    }
                }
                TextureManager.TextureProgram.SetTextureColor(Color4.White);
                TextureManager.TextureProgram.PopModelView();
            }
        }

        public Loc2D GetStart() {
            return new Loc2D();
        }

        public Loc2D GetEnd() {
            return new Loc2D();
        }

    }
}