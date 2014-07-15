using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Logic.Display
{

    class CharacterOriginAnimation : ISprite
    {
        #region Constructors

        public CharacterOriginAnimation(int charIndex, int animIndex, RenderTime animTime, int loops)
        {
            AnimationIndex = animIndex;
            FrameLength = animTime;
            TotalLoops = loops;
            CharIndex = charIndex;
        }

        #endregion Constructors

        #region Properties

        public int CharIndex { get; set; }

        public int AnimationIndex
        {
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

        public int Frame
        {
            get;
            set;
        }

        //total frames

        public int Loops
        {
            get;
            set;
        }

        public int TotalLoops
        {
            get;
            set;
        }

        public MoveAnimationType AnimType
        {
            get { return MoveAnimationType.Normal; }
        }

        public Direction8 Direction { get { return Direction8.None; } }

        public Loc2D MapLoc { get; set; }
        public int MapHeight { get; set; }

        public RenderTime ActionTime { get; set; }
        public bool ActionDone { get; set; }


        #endregion Properties

        public virtual void Begin()
        {
            CharSprite sprite = null;
            if (CharIndex < 0)
            {
                sprite = Screen.Players[CharIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
            }
            else
            {
                sprite = Screen.Npcs[CharIndex];
            }
            MapLoc = sprite.MapLoc;
        }

        public virtual void Process(RenderTime elapsedTime)
        {
            ActionTime += elapsedTime;
            FrameTime += elapsedTime;
            if (FrameTime >= FrameLength)
            {
                FrameTime = FrameTime - FrameLength;
                Frame++;
            }

            if (Frame >= TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TotalFrames)
            {
                Loops++;
                Frame = 0;
            }

            if (Loops >= TotalLoops)
            {
                ActionDone = true;
            }
        }

        public virtual void Draw()
        {
            if (!ActionDone)
            {
                TextureManager.TextureProgram.PushModelView();
                Loc2D drawLoc = GetStart();
                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(drawLoc.X, drawLoc.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                AnimSheet sheet = TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex);
                sheet.RenderAnim(Frame, 0, 0);

                TextureManager.TextureProgram.PopModelView();
            }
        }

        public Loc2D GetStart()
        {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileHeight / 2 - MapHeight);
        }

        public Loc2D GetEnd()
        {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetSpellSheet(TextureManager.SpellAnimType.Spell, AnimationIndex).TileHeight / 2 - MapHeight);
        }

    }
}