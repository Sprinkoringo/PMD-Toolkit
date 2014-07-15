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

    class TileMoveAnimation : IEmitter {
        #region Constructors

        public TileMoveAnimation(Loc2D startLoc, int animIndex, RenderTime animTime, int loops, Enums.RangeType rangeType, Maps.Direction8 dir, int range, RenderTime stallTime)
        {
            AnimationIndex = animIndex;
            FrameLength = animTime;
            TotalLoops = loops;
            StartLoc = startLoc;
            RangeType = rangeType;
            Direction = dir;
            TotalDistance = range;
            StallTime = stallTime;

            FrameTime = StallTime;
        }

        #endregion Constructors

        #region Properties

        public RenderTime TotalTime
        {
            get
            {
                return StallTime * TotalDistance;
            }
        }

        public int AnimationIndex {
            get;
            set;
        }


        public RenderTime StallTime
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

        //total frames
        
        public int TotalLoops {
            get;
            set;
        }

        public MoveAnimationType AnimType {
            get { return MoveAnimationType.Tile; }
        }

        public Loc2D StartLoc {
            get;
            set;
        }

        public Enums.RangeType RangeType {
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


        //public Loc2D MapLoc { get { return StartLoc * TextureManager.TILE_SIZE; } }
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
            if (FrameTime >= StallTime && Distance < TotalDistance) {
                FrameTime = FrameTime - StallTime;

                Distance++;
                //add new layer of animations
                List<Loc2D> targetTiles = Gameplay.Processor.GetExclusiveTiles(StartLoc, Direction, RangeType, Distance);
                foreach (Loc2D loc in targetTiles) {
                    Display.Screen.Effects[Screen.EffectPriority.None].Add(new NormalMoveAnimation(loc, AnimationIndex, FrameLength, TotalLoops));
                }
                if (Distance >= TotalDistance) {
                    ActionDone = true;
                }
            }
        }

    }
}