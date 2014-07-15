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

    class ItemArrowMoveAnimation : ISpellSprite {
        #region Constructors

        public ItemArrowMoveAnimation(Loc2D startLoc, int animIndex, RenderTime animTime, Maps.Direction8 dir, int distance, int speed)
        {
            StartLoc = startLoc;
            AnimationIndex = animIndex;
            FrameLength = animTime;
            Direction = dir;
            TotalWaves = distance;
            TotalDistance = distance * TextureManager.TILE_SIZE;
            TravelSpeed = speed;
        }

        #endregion Constructors

        #region Properties

        public int TotalTime
        {
            get
            {
                return TotalDistance * 1000 / TravelSpeed;
            }
        }

        public int TotalWaves
        {
            get;
            private set;
        }

        public int AnimationIndex {
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

        public MoveAnimationType AnimType {
            get { return MoveAnimationType.ItemArrow; }
        }

        public Loc2D StartLoc {
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

            if (Frame >= TextureManager.GetItemSheet(AnimationIndex).TotalFrames) {
                Frame = 0;
            }

            Distance = ActionTime.ToMillisecs() * TravelSpeed / 1000;

            if (Distance >= TotalDistance) {
                ActionDone = true;
            } else {
                Loc2D mapLoc = new Loc2D(StartLoc.X * TextureManager.TILE_SIZE, StartLoc.Y * TextureManager.TILE_SIZE);
                Operations.MoveInDirection8(ref mapLoc, Direction, Distance);
                MapLoc = mapLoc;
            }
        }

        public virtual void Draw() {
            if (!ActionDone) {
                TextureManager.TextureProgram.PushModelView();
                Loc2D drawLoc = GetStart();

                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(drawLoc.X, drawLoc.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                AnimSheet sheet = TextureManager.GetItemSheet(AnimationIndex);
                sheet.RenderAnim(Frame, 0, 0);

                TextureManager.TextureProgram.PopModelView();
            }
        }

        public Loc2D GetStart() {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetItemSheet(AnimationIndex).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetItemSheet(AnimationIndex).TileHeight / 2 - MapHeight);
        }

        public Loc2D GetEnd() {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetItemSheet(AnimationIndex).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetItemSheet(AnimationIndex).TileHeight / 2 - MapHeight);
        }

    }
}