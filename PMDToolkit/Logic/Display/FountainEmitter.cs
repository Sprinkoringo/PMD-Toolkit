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

    public class FountainEmitter : IEmitter {
        #region Constructors

        public FountainEmitter(Loc2D startLoc, int animIndex, int grainsPerBurst, RenderTime burstTime, int bursts, RenderTime animTime, int startDistance, int speed, RenderTime totalTime)
        {
            StartLoc = startLoc;
            AnimationIndex = animIndex;
            FrameLength = animTime;
            TotalTime = totalTime;
            GrainsPerBurst = grainsPerBurst;
            BurstTime = burstTime;
            TotalBursts = bursts;
            StartDistance = startDistance;
            Speed = speed;
        }

        #endregion Constructors

        #region Properties

        public int AnimationIndex {
            get;
            set;
        }
        
        public RenderTime FrameLength {
            get;
            set;
        }

        public RenderTime TotalTime
        {
            get;
            set;
        }


        public Loc2D StartLoc {
            get;
            set;
        }

        public int StartDistance { get; set; }

        public int Speed {
            get;
            set;
        }

        public int GrainsPerBurst { get; set; }

        public RenderTime CurrentBurstTime { get; set; }
        public RenderTime BurstTime { get; set; }
        public int Bursts { get; set; }
        public int TotalBursts { get; set; }


        public RenderTime ActionTime { get; set; }
        public bool ActionDone { get; set; }

        #endregion Properties


        public virtual void Process(RenderTime elapsedTime) {
            ActionTime += elapsedTime;
            CurrentBurstTime += elapsedTime;
            if (CurrentBurstTime >= BurstTime) {
                CurrentBurstTime -= BurstTime;
                for (int i = 0; i < GrainsPerBurst; i++) {
                    double angle = Logic.Display.Screen.Rand.NextDouble() * MathHelper.TwoPi;
                    Loc2D particleSpeed = new Loc2D((int)(Math.Cos(angle) * Speed), (int)(Math.Sin(angle) * Speed));
                    int dist = Logic.Display.Screen.Rand.Next(StartDistance + 1);
                    Loc2D startDelta = new Loc2D((int)(Math.Cos(angle) * dist), (int)(Math.Sin(angle) * dist));
                    Display.Screen.Effects[Screen.EffectPriority.None].Add(new ParticleAnimation(AnimationIndex, FrameLength, StartLoc + startDelta, particleSpeed, new Loc2D(), Color4.White, Color4.Gray, TotalTime ));
                }
                Bursts++;
            }


            if (Bursts >= TotalBursts) {
                ActionDone = true;
            }
        }


    }
}