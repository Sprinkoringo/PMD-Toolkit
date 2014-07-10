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
    public class ParticleAnimation : ISpellSprite {

        //float scale;
        //float scaleSpeed;
        //float rotation;
        //float rotationSpeed;


        public ParticleAnimation(int animationIndex, RenderTime frameLength, Loc2D newPosition, Loc2D newSpeed, Loc2D newAcceleration, Color4 newAlpha, Color4 newAlphaSpeed, RenderTime newMaxTime)
        {
            AnimationIndex = animationIndex;
            FrameLength = frameLength;
            MapLoc = newPosition;
            StartLoc = MapLoc;
            Speed = newSpeed;
            StartSpeed = Speed;
            Acceleration = newAcceleration;
            Color = newAlpha;
            ColorChange = newAlphaSpeed;
            TotalTime = newMaxTime;
        }


        #region Properties

        public int AnimationIndex {
            get;
            set;
        }


        public RenderTime FrameTime
        {
            get;
            set;
        }

        public RenderTime FrameLength
        {
            get;
            set;
        }

        public int Frame {
            get;
            set;
        }

        public RenderTime TotalTime { get; set; }

        public MoveAnimationType AnimType {
            get { return MoveAnimationType.Particle; }
        }

        public Loc2D StartLoc {
            get;
            set;
        }

        public Loc2D StartSpeed { get; set; }
        public Loc2D Speed { get; set; }
        public Loc2D Acceleration { get; set; }

        public Color4 Color { get; set; }
        public Color4 ColorChange { get; set; }

        public Direction8 Direction { get { return Direction8.None; } }

        public Loc2D MapLoc { get; set; }
        public int MapHeight { get { return TextureManager.TILE_SIZE / 2; } }

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
                Frame = 0;
            }

            MapLoc = StartLoc + Speed * ActionTime.ToMillisecs() / 1000;
            Speed = StartSpeed + Acceleration * ActionTime.ToMillisecs() / 1000;
            if (ActionTime >= TotalTime) {
                ActionDone = true;
            }
        }

        public virtual void Draw() {
            if (!ActionDone) {
                TextureManager.TextureProgram.PushModelView();
                Loc2D drawLoc = GetStart();
                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(drawLoc.X, drawLoc.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                AnimSheet sheet = TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex);
                sheet.RenderAnim(Frame, 0, 0);

                TextureManager.TextureProgram.PopModelView();
            }
        }

        public Loc2D GetStart() {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileHeight / 2 - MapHeight);
        }

        public Loc2D GetEnd() {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileHeight / 2 - MapHeight);
        }

    }
}
