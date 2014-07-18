/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

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
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Logic.Display {

    class ThrowMoveAnimation : ISpellSprite {
        #region Constructors

        public ThrowMoveAnimation(Loc2D startLoc, Loc2D endLoc, int animIndex, RenderTime animTime, int speed, bool dropDown)
        {
            StartLoc = startLoc;
            EndLoc = endLoc;
            AnimationIndex = animIndex;
            FrameLength = animTime;
            Loc2D diffLoc = startLoc-endLoc;
            TotalDistance = (int)(TextureManager.TILE_SIZE*Math.Sqrt(Math.Pow(diffLoc.X,2) + Math.Pow(diffLoc.Y,2)));
            TravelSpeed = speed;
            DropDown = dropDown;
        }

        #endregion Constructors

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

        //total frames

        public MoveAnimationType AnimType {
            get { return MoveAnimationType.Throw; }
        }

        public Loc2D StartLoc {
            get;
            set;
        }

        public Loc2D EndLoc {
            get;
            set;
        }

        public int TravelSpeed {
            get;
            set;
        }

        public int Distance {
            get;
            set;
        }

        public int TotalDistance {
            get;
            set;
        }

        public Maps.Direction8 Direction {
            get;
            set;
        }

        public bool DropDown { get; set; }

        public Loc2D MapLoc { get; set; }
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
                Frame = 0;
            }

            Distance = ActionTime.ToMillisecs() * TravelSpeed;
            
            if (Distance >= TotalDistance) {
                ActionDone = true;
            } else {
                MapHeight = DrawHelper.GetArc(TotalDistance, TotalDistance, Distance);

                if (DropDown) {
                    MapHeight += TextureManager.TILE_SIZE * (TotalDistance - Distance) / TotalDistance / 2;
                } else {
                    MapHeight += TextureManager.TILE_SIZE / 2;
                }

                Loc2D mapDiff = (EndLoc - StartLoc) * TextureManager.TILE_SIZE;
                mapDiff = new Loc2D(mapDiff.X * Distance / TotalDistance, mapDiff.Y * Distance / TotalDistance);

                MapLoc = mapDiff + StartLoc * TextureManager.TILE_SIZE;
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